using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using smartFactory_MillProcess.Models;
using ZstdSharp.Unsafe;
using System.Reflection.Emit;
using Mysqlx;
using Org.BouncyCastle.Crypto.Generators;

namespace smartFactory_MillProcess.ViewModels
{
    public partial class RollingMachineViewModel : ObservableObject
    {
        public bool IsMenuOpen { get; set; }
        private RollingMachine rollingMachineModel = new RollingMachine();
        private DispatcherTimer timer;

        private int elapsedSeconds;  // 🔹 경과 시간
        private Random random = new Random();  // 🔹 난수 생성기

        [ObservableProperty]
        private int rollSpeed;
        [ObservableProperty]
        private double initialThickness;
        [ObservableProperty]
        private double finalThickness;
        [ObservableProperty]
        private double hardness;
        [ObservableProperty]
        private double strength;
        [ObservableProperty]
        private double compressionRatio;
        [ObservableProperty]
        private double errorRatio;
        [ObservableProperty]
        private int errors = 1;
        [ObservableProperty]
        private int completeCount = 2;
        [ObservableProperty]
        private double averageTemperature;

        [ObservableProperty]
        private string userInput;
        [ObservableProperty]
        private double progressValue;
        [ObservableProperty]
        private string selectedMaterial;
        [ObservableProperty]
        private int minAllowedSped = 10;

        [ObservableProperty]
        private int maxAllowedSped = 0;

        // app.xaml.cs 에서 온도 전달받음
        public void AverageTemperatureFromFurnace()
        {
            double averageTemperature = App.FurnaceVM.AverageTemperature;

        }


        public ObservableCollection<string> MaterialOptions { get; } = new ObservableCollection<string>
        {
            "Al5082",
            "SUS304",
            "SM45C"
        };

        public RollingMachineViewModel()
        {

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += UpdateRollSpeed;
            rollingMachineModel = new RollingMachine();
        }

        partial void OnSelectedMaterialChanged(string value)
        {
            switch (value)
            {
                case "Al5082":
                    MinAllowedSped = 1;
                    MaxAllowedSped = 10;
                    initialThickness = 300;
                    MessageBox.Show($"{MinAllowedSped}~{MaxAllowedSped} 사이의 값을 입력하세요");
                    break;
                case "SUS304":
                    MinAllowedSped = 1;
                    MaxAllowedSped = 10;
                    initialThickness = 200;
                    MessageBox.Show($"{MinAllowedSped}~{MaxAllowedSped} 사이의 값을 입력하세요");
                    break;
                case "SM45C":
                    MinAllowedSped = 1;
                    MaxAllowedSped = 10;
                    initialThickness = 270;
                    MessageBox.Show($"{MinAllowedSped}~{MaxAllowedSped} 사이의 값을 입력하세요");
                    break;
                default:
                    MinAllowedSped = 0;
                    MaxAllowedSped = 0;
                    initialThickness = 0;
                    MessageBox.Show("재료를 선택해주세요");
                    break;
            }
        }

        [RelayCommand]
        private void StartRollSpeedUpdate()
        {

            if (int.TryParse(UserInput, out int userSpeed) && userSpeed >= MinAllowedSped && userSpeed <= MaxAllowedSped)
            {
                if (timer.IsEnabled)
                {
                    timer.Stop(); //  기존 타이머 중단
                }

                RollSpeed = userSpeed;

                //  타이머 및 기록 초기화
                elapsedSeconds = 0;

                ProgressValue = 0;

                timer.Start();
            }
            else
            {
                MessageBox.Show($"⚠ {MinAllowedSped}~{MaxAllowedSped}도 사이의 숫자를 입력하세요!");
            }
        }

        private void UpdateRollSpeed(object? sender, EventArgs e)
        {
            if (elapsedSeconds <= 60)
            {
                ProgressValue = (elapsedSeconds * 100) / 60;
                elapsedSeconds++;
            }
            else
            {
                timer.Stop();
                CompleteCount++;

                FinalThickness = CaculateFinalThickness();
                Hardness = CaculateHardness();
                Strength = CaculateStrength();
                CompressionRatio = CaculateCompressionRatio();

                if (CheckoutError() == true)
                {
                    Errors++;
                }
                ErrorRatio = CaculateErrorRatio();
                MessageBox.Show($"{CompleteCount} Errors: {Errors}");

                DefectResult = CheckError(rollSpeed, averageTemperature) ? "불량 발생" : "양호";
            }
        }

        private double CaculateFinalThickness()
        {
            return 15;
        }

        private double CaculateHardness()
        {
            return 30;
        }

        private double CaculateStrength()
        {
            return 40;
        }

        private double CaculateCompressionRatio()
        {
            return 13;
        }
        private double CaculateErrorRatio()
        {
            ErrorRatio = (double)Errors / CompleteCount;
            return ErrorRatio;
        }
        private bool CheckoutError()
        {
            return true;
        }

        private Dictionary<string, (double A, double B, double c, double D, double H0, double k, double σref, double k2, double Tref)> RConst = new()
        {
            { "Al5082", (0.65, 0.3, 0.0025, 0.037, 80, 0.0024, 150, 0.0025, 298) },   // 알루미늄
            { "SUS304", (0.6, 0.35, 0.0028, 0.012, 210, 0.0026, 280, 0.002, 298) },   // 스테인리스
            { "SM45C",  (0.55, 0.4, 0.003, 0.02, 190, 0.0028, 350, 0.0018, 298) }    // 탄소강
        };
        // 최종 두께
        private double CalcFinalThickness(double rollSpeed, double averageTemperature)
        {
            double averageTempKelvin = averageTemperature + 273.15;

            if (RConst.TryGetValue(SelectedMaterial, out var constants))
            {
                double A = constants.A;
                double B = constants.B;
                double c = constants.c;
                double D = constants.D;
                finalThickness = initialThickness * (A + (B * Math.Exp(-c * averageTempKelvin) + D * Math.Log(rollSpeed)));
            }

            return 0;
        }
        // 경도
        private double Calchardness(double averageTemperature)
        {
            double averageTempKelvin = averageTemperature + 273.15;

            if (RConst.TryGetValue(SelectedMaterial, out var constants))
            {
                double H0 = constants.H0;
                double k = constants.B;
                hardness = H0 * (Math.Exp(-k * averageTempKelvin));
            }

            return 0;
        }
        // 강도
        private double Calcstrength(double averageTemperature)
        {
            double averageTempKelvin = averageTemperature + 273.15;

            if (RConst.TryGetValue(SelectedMaterial, out var constants))
            {
                double σref = constants.σref;
                double k2 = constants.k2;
                double Tref = constants.Tref;
                strength = σref + Math.Exp(-k2 * (averageTempKelvin - Tref));
            }

            return 0;
        }
        // 압하율
        private double CalcCompressionRatio(double initialThickness, double finalThickness)
        {
            double averageTempKelvin = averageTemperature + 273.15;
            if (RConst.TryGetValue(SelectedMaterial, out var constants))
            {
                compressionRatio = (((initialThickness - finalThickness) / initialThickness) * 100) ;
            }
            return 0;
        }

        // 불량 판별
        private bool CheckError(double rollSpeed, double averageTemperature)
        {
            double averageTempKelvin = averageTemperature + 273.15;
            if (RConst.TryGetValue(SelectedMaterial, out var constants))
            {
                // 불량 판별 로직
                if (compressionRatio < 99.6 || compressionRatio > 79.6)
                {
                    return true; // 불량 발생
                }
            }
            return false; // 불량 없음
        }

        private string _defectResult = "";
        public string DefectResult
        {
            get => _defectResult;
            set
            {
                _defectResult = value;
                OnPropertyChanged(nameof(DefectResult));
            }
        }
    }
}


