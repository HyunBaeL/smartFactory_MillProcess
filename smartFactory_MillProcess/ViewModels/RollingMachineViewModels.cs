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
using ScottPlot;
using ScottPlot.WPF;
using ScottPlot.Reporting;
using System.Reflection.Emit;
using Mysqlx;
using smartFactory_MillProcess.Repositories;
using System.Windows.Media;

namespace smartFactory_MillProcess.ViewModels
{
    public partial class RollingMachineViewModel : ObservableObject
    {
        private RollingMachineRepository rollingRepo = new RollingMachineRepository();
        public MachineStatusRepository machineStatusRepository {  get; set; } = new MachineStatusRepository();
        MachineViewModel machineViewModel = new MachineViewModel();

        public bool IsMenuOpen { get; set; }
        private RollingMachine rollingMachineModel = new RollingMachine();
        private MachineStatus machineStatus = new MachineStatus();

        [ObservableProperty]
        public int machineProcessCount;

        [ObservableProperty]
        private Brush machineBackground = Brushes.Transparent;

        private DispatcherTimer timer;

        private int elapsedSeconds;  // 🔹 경과 시간
        private Random random = new Random();  // 🔹 난수 생성기

        [ObservableProperty]
        private double rollSpeed;
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
        private int minAllowedSped = 0;
        [ObservableProperty]
        private int recommendedSpeed;


        [ObservableProperty]
        private int maxAllowedSped = 0;

        // app.xaml.cs 에서 온도 전달받음
        public void AverageTemperatureFromFurnace()
        {
            AverageTemperature = App.FurnaceVM.AverageTemperature;

        }
        //// app.xaml.cs 에서 재료 선택 전달받음
        //partial void OnSelectedMaterialChanged(string value)
        //{
        //    App.FurnaceVM.SelectedMaterial = value;
        //}

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
            
        }

        partial void OnSelectedMaterialChanged(string value)
        {
            App.FurnaceVM.SelectedMaterial = value;

            switch (value)
            {
                case "Al5082":
                    MinAllowedSped = 5;
                    MaxAllowedSped = 10;
                    InitialThickness = 300;
                    RecommendedSpeed = 7;
                    //MessageBox.Show($"{MinAllowedSped}~{MaxAllowedSped} 사이의 값을 입력하세요");
                    break;
                case "SUS304":
                    MinAllowedSped = 1;
                    MaxAllowedSped = 3;
                    InitialThickness = 200;
                    RecommendedSpeed = 2;
                    //MessageBox.Show($"{MinAllowedSped}~{MaxAllowedSped} 사이의 값을 입력하세요");
                    break;
                case "SM45C":
                    MinAllowedSped = 4;
                    MaxAllowedSped = 6;
                    InitialThickness = 270;
                    RecommendedSpeed = 5;
                    //MessageBox.Show($"{MinAllowedSped}~{MaxAllowedSped} 사이의 값을 입력하세요");
                    break;
                default:
                    MinAllowedSped = 0;
                    MaxAllowedSped = 0;
                    InitialThickness = 0;
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
                MessageBox.Show($"⚠ Roll Speed {MinAllowedSped}~{MaxAllowedSped} 사이의 숫자를 입력하세요!");
            }
        }

        private async void UpdateRollSpeed(object? sender, EventArgs e)
        {
            if (elapsedSeconds <= 7)
            {
                ProgressValue = (elapsedSeconds * 100) / 7;
                elapsedSeconds++;
            }
            else
            {
                timer.Stop();
                CompleteCount++;

                FinalThickness = CalcFinalThickness(RollSpeed,AverageTemperature);
                Hardness = CalcHardness(AverageTemperature);
                Strength = CalcStrength(AverageTemperature);
                CompressionRatio = CalcCompressionRatio(InitialThickness, FinalThickness);

                //if (CheckoutError() == true)
                //{
                //    Errors++;
                //}
                //ErrorRatio = CaculateErrorRatio();
                //MessageBox.Show($"{CompleteCount} Errors: {Errors}");

                MessageBox.Show("압연기 작업이 완료되었습니다.");

                bool isError = CheckError(RollSpeed, AverageTemperature);
                DefectResult = isError ? "불량" : "양호";

                if (isError)
                {
                    _ = BlinkBackgroundAsync("Red"); // 깜빡이게
                }
                else
                {
                    _ = BlinkBackgroundAsync("Green");
                }


                await InsertMachineStatus(isError);
                MachineProcessCount = await machineStatusRepository.SelectTodayTotalCount();
            }
        }

        
        //private double CaculateErrorRatio()
        //{
        //    ErrorRatio = (double)Errors / CompleteCount;
        //    return ErrorRatio;
        //}
        //private bool CheckoutError()
        //{
        //    return true;
        //}

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
                return FinalThickness = InitialThickness * (A + (B * Math.Exp(-c * averageTempKelvin) + D * Math.Log(rollSpeed)));
            }

            return 0;
        }
        // 경도
        private double CalcHardness(double averageTemperature)
        {
            double averageTempKelvin = averageTemperature + 273.15;

            if (RConst.TryGetValue(SelectedMaterial, out var constants))
            {
                double H0 = constants.H0;
                double k = constants.k;
                return Hardness = H0 * (Math.Exp(-k * averageTempKelvin));
            }

            return 0;
        }
        // 강도
        private double CalcStrength(double averageTemperature)
        {
            double averageTempKelvin = averageTemperature + 273.15;

            if (RConst.TryGetValue(SelectedMaterial, out var constants))
            {
                double σref = constants.σref;
                double k2 = constants.k2;
                double Tref = constants.Tref;
                return Strength = σref + Math.Exp(-k2 * (averageTempKelvin - Tref));
            }

            return 0;
        }
        // 압하율
        private double CalcCompressionRatio(double initialThickness, double finalThickness)
        {
            double averageTempKelvin = AverageTemperature + 273.15;
            if (RConst.TryGetValue(SelectedMaterial, out var constants))
            {
                return CompressionRatio = (initialThickness - finalThickness) / initialThickness * 100;
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
                if (CompressionRatio < 20 || CompressionRatio > 40)
                {
                    machineStatus.ThicknessResult = FinalThickness;
                    machineStatus.HardnessResult = Hardness;
                    machineStatus.StrenghResult = Strength;
                    machineStatus.ReductionRatidResult = CompressionRatio;
                    machineStatus.DefectStatus = true;

                    return true; // 불량 발생
                }
            }

            machineStatus.ThicknessResult = FinalThickness;
            machineStatus.HardnessResult = Hardness;
            machineStatus.StrenghResult = Strength;
            machineStatus.ReductionRatidResult = CompressionRatio;
            machineStatus.DefectStatus = false;

            return false; // 불량 없음
        }

        private async Task InsertMachineStatus(bool errorCheck)
        {
            if (errorCheck)
            {
                machineStatus = await rollingRepo.InsertMachineStatus(machineStatus); // 불량시
                MachineProcessCount = await machineStatusRepository.SelectTodayTotalCount();
            }
            else
            {
                machineStatus = await rollingRepo.InsertMachineStatus(machineStatus); // 정상품일시
            }
        }

        private CancellationTokenSource? blinkCancellation;

        private async Task BlinkBackgroundAsync(string color)
        {
            blinkCancellation?.Cancel(); // 기존 깜빡임 중단
            blinkCancellation = new CancellationTokenSource();
            var token = blinkCancellation.Token;

            try
            {
                for (int i = 0; i < 15; i++) // 6번 깜빡이면 약 3초
                {
                    if (color.Equals("Red"))
                    {
                        MachineBackground = Brushes.Red;
                        await Task.Delay(250, token);
                        MachineBackground = Brushes.Transparent;
                        await Task.Delay(250, token);
                    }
                    else
                    {
                        MachineBackground = Brushes.Green;
                        await Task.Delay(250, token);
                        MachineBackground = Brushes.Transparent;
                        await Task.Delay(250, token);
                    }
                    
                }
            }
            catch (TaskCanceledException)
            {
                // 아무 것도 하지 않음
            }
            finally
            {
                MachineBackground = Brushes.Transparent;
            }
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


