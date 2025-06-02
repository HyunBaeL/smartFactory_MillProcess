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
        private int completeCount= 2;

        [ObservableProperty]
        private string userInput;
        [ObservableProperty]
        private double progressValue;
        [ObservableProperty]
        private string selectedMaterial;
        [ObservableProperty]
        private int minAllowedSped = 0;

        [ObservableProperty]
        private int maxAllowedSped = 0;

        public ObservableCollection<string> MaterialOptions { get; } = new ObservableCollection<string>
        {
            "알루미늄",
            "스테인리스",
            "탄소강"
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
                case "알루미늄":
                    MinAllowedSped = 900;
                    MaxAllowedSped = 1100;
                    MessageBox.Show($"{MinAllowedSped}~{MaxAllowedSped} 사이의 값을 입력하세요");
                    break;
                case "스테인리스":
                    MinAllowedSped = 1100;
                    MaxAllowedSped = 1200;
                    MessageBox.Show($"{MinAllowedSped}~{MaxAllowedSped} 사이의 값을 입력하세요");
                    break;
                case "탄소강":
                    MinAllowedSped = 1200;
                    MaxAllowedSped = 1300;
                    MessageBox.Show($"{MinAllowedSped}~{MaxAllowedSped} 사이의 값을 입력하세요");
                    break;
                default:
                    MinAllowedSped = 0;
                    MaxAllowedSped = 0;
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
        
        

        


    }
}


