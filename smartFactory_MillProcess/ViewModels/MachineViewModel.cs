using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveCharts;
using LiveCharts.Wpf;
using CommunityToolkit.Mvvm.ComponentModel;
using smartFactory_MillProcess.Repositories;
using smartFactory_MillProcess.Models;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using smartFactory_MillProcess.Views;

namespace smartFactory_MillProcess.ViewModels
{
    public partial class MachineViewModel : ObservableObject
    {
        private MachineRepository machineRepo = new MachineRepository();

        [ObservableProperty]
        private Machine mc = new Machine();

        [ObservableProperty]
        private ObservableCollection<string> xLabel;
        public string[] XLabelArray => XLabel.ToArray();

        [ObservableProperty]
        public SeriesCollection seriesData;

        [ObservableProperty]
        private double? measuredTemperature;

        [ObservableProperty]
        private double? humidity;
        [ObservableProperty]
        private int contamination_level;
        [ObservableProperty]
        private string? repareText = "양호";
        [ObservableProperty]
        private string? durationText = "양호";

        private DispatcherTimer Timer;

        public Func<double, string> ValuesFormatter { get; set; }

        private bool isWorkEnvironmentStarted = false;

        public MachineViewModel()
        {
            Mc = Mc ?? new Machine();

            if (!isWorkEnvironmentStarted)
            {
                isWorkEnvironmentStarted = true;
                Task.Run(async () => await WorkEnvironmentUpdate());
            }

            seriesData = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "온도",
                    Values = new ChartValues<double> { Mc.Temperature },
                    PointGeometry = DefaultGeometries.Circle,


                },
                new LineSeries
                {
                    Title = "습도",
                    Values = new ChartValues<double> { Mc.Humidity },
                    PointGeometry = DefaultGeometries.Square
                },
                new LineSeries
                {
                    Title = "오염도",
                    Values = new ChartValues<int> {Mc.Contamination_level},
                    PointGeometry = DefaultGeometries.Square
                }
            };

            xLabel = new ObservableCollection<string> { };

            // ✔ LabelFormatter 함수 정의
            ValuesFormatter = val => $"{val:F1}";
        }

        private bool isUpdating = false;

        private async Task WorkEnvironmentUpdate()
        {
            if (isUpdating)
                return;

            isUpdating = true;

            try
            {
                while (true)
                {
                    await Task.Delay(TimeSpan.FromSeconds(5));

                    //값 측정
                    MeasuredTemperature = TemperatureMesure(20.0);
                    Humidity = HumidityMesure(30.0);
                    Contamination_level = PollutionMeasure(0);

                    // Mc 객체가 null인 경우 방어적 처리
                    if (Mc == null)
                    {
                        Mc = new Machine();  // 새 객체로 초기화
                    }

                    Mc.Temperature = MeasuredTemperature.Value;
                    Mc.Humidity = Humidity.Value;
                    Mc.Contamination_level = Contamination_level;
                    Mc.Repare_status = false;
                    Mc.Durability = true;

                    // DB에 값 삽입
                    Mc = await machineRepo.InsertMuchine(Mc);

                    // 현재 시간을 구합니다.
                    DateTime currentTime = DateTime.Now;

                    // 시간과 분을 추출합니다.
                    int currentHour = currentTime.Hour;
                    int currentMinute = currentTime.Minute;

                    // 시간 증가 및 레이블 업데이트
                    int timeIndex = 0;
                    string label = $"{currentHour}시 {currentMinute}분";

                    // UI 스레드에서 xLabel 업데이트
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        xLabel.Add(label);

                        // X축 레이블도 추가하고 갱신
                        if (xLabel.Count > 8)
                            xLabel.RemoveAt(0);
                    });

                    // 그래프 
                    // 그래프 업데이트
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        // 온도 그래프 값 추가
                        seriesData[0].Values.Add(Mc.Temperature);
                        if (seriesData[0].Values.Count > 8)
                            seriesData[0].Values.RemoveAt(0);

                        // 습도 그래프 값 추가
                        seriesData[1].Values.Add(Mc.Humidity);
                        if (seriesData[1].Values.Count > 8)
                            seriesData[1].Values.RemoveAt(0);

                        // 오염도 그래프 값 추가
                        seriesData[2].Values.Add(Mc.Contamination_level);
                        if (seriesData[2].Values.Count > 8)
                            seriesData[2].Values.RemoveAt(0);
                    });

                }
            }
            finally
            {
                isUpdating = false;
            }
        }

        private double TemperatureMesure(double mean)
        {
            double standardTemperature = 0.7;  // 표준 편차

            Random ran = new Random();
            double randomTemperature = ran.NextDouble();
            double resultTemperature = mean + standardTemperature * InverseNormal(randomTemperature);

            // 소수 첫째자리까지 반올림하여 리턴
            return Math.Round(resultTemperature, 1);
        }

        /*
         * 정교한 온도 편차 표현을 위한 메소드
         */
        private double InverseNormal(double p)
        {
            return Math.Sqrt(-2 * Math.Log(1 - p)) * Math.Cos(2 * Math.PI * p);
        }

        private double HumidityMesure(double mean)
        {
            double standardHumidity = 10.0;  // 표준 편차 (습도에 맞게 조정)

            Random ran = new Random();
            double randomHumidity = ran.NextDouble();
            double resultHumidity = mean + standardHumidity * InverseNormal(randomHumidity);

            // 습도는 0~100 범위로 제한
            resultHumidity = Math.Max(0, Math.Min(resultHumidity, 100));

            // 소수 첫째자리까지 반올림하여 리턴
            return Math.Round(resultHumidity, 1);
        }

        /*
         * 랜덤 오염도 추출 메소드
         */
        private int PollutionMeasure(int mean)
        {
            double standardPollution = 150;  // 표준 편차 (오염도에 맞게 조정)

            Random ran = new Random();
            double randomPollution = ran.NextDouble();  // 0.0과 1.0 사이의 실수 생성
            double resultPollution = mean + standardPollution * InverseNormal(randomPollution);

            // 오염도는 0~1000 범위로 제한
            resultPollution = Math.Max(0, Math.Min(resultPollution, 1000));

            // 소수 첫째 자리까지만 반올림하여 정수로 리턴
            return (int)Math.Round(resultPollution, 1);
        }
    }
}
