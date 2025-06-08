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
        //private RollingMachineViewModel rollingMachineViewModel = new RollingMachineViewModel();

        private MachineStatusRepository machineStatusRepository = new MachineStatusRepository();

        [ObservableProperty]
        private int? machineProcessCount;

        [ObservableProperty]
        private string machineProcessCountFormatted;

        [ObservableProperty]
        private int weekProcessCount;
        [ObservableProperty]
        private string weekProcessCountFormat;

        [ObservableProperty]
        private int todayProcessCount;
        [ObservableProperty]
        private string todayProcessCountFormat;

        [ObservableProperty]
        private int monthProcessCount;
        [ObservableProperty]
        private string monthProcessCountFormat;

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

        [ObservableProperty]
        public SeriesCollection newSeries;

        [ObservableProperty]
        public ObservableCollection<string> newXLabel;

        private List<(DateTime Time, int Count)> _timeSeriesData = new();


        public Func<double, string> NewValuesFormatter { get; set; }

        private DispatcherTimer Timer;

        public Func<double, string> ValuesFormatter { get; set; }

        private bool isWorkEnvironmentStarted = false;

        public MachineViewModel()
        {

            Mc = Mc ?? new Machine();

            if (!isWorkEnvironmentStarted)
            {
                isWorkEnvironmentStarted = true;

                Task.Run(async () =>
                {
                    try
                    {
                        await WorkEnvironmentUpdate();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                });

                //Task.Run(async () =>
                //{
                //    try
                //    {
                //        MachineProcessCount = await machineStatusRepository.SelectTodayTotalCount();
                //    }
                //    catch (Exception ex)
                //    {
                //        MessageBox.Show(ex.Message);
                //    }
                //});

            }

            UpdateGraph();

        }

        private void UpdateGraph()
        {
            SeriesData = new SeriesCollection
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

            XLabel = new ObservableCollection<string> { };

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
                    MachineProcessCount = await machineStatusRepository.SelectTodayTotalCount();
                    //MachineProcessCountFormatted = MachineProcessCount.ToString() + " / 250";

                    TodayProcessAmount();

                    DateTimeAndGraphUpdate();


                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

            }
            finally
            {
                isUpdating = false;
            }
        }

        private DateTime lastWeekReset = DateTime.MinValue; // 마지막 주간 초기화 날짜
        private DateTime lastMonthReset = DateTime.MinValue; // 마지막 월간 초기화 날짜
        private DateTime lastDate = DateTime.MinValue; // 날짜가 바꼈을때

        private int WeekCount = 0;
        private int MonthCount = 0;

        private void TodayProcessAmount()
        {
            // 오늘 날짜가 변경되었을 때 TodayProcessCount 초기화
            if (lastDate.Date != DateTime.Now.Date)
            {
                WeekCount += TodayProcessCount;
                TodayProcessCount = 0;  // 새로운 날로 초기화
                lastDate = DateTime.Now; // 마지막 날짜 갱신
            }

            // 일주일마다 WeekProcessCount 초기화
            if (lastWeekReset.Date != DateTime.Now.Date && DateTime.Now.DayOfYear - lastWeekReset.DayOfYear >= 7)
            {
                MonthCount += WeekCount;
                WeekCount = 0;
                WeekProcessCount = 0;
                lastWeekReset = DateTime.Now; // 마지막 주간 초기화 날짜 갱신
            }

            // 한 달마다 MonthProcessCount 초기화
            if (lastMonthReset.Month != DateTime.Now.Month || lastMonthReset.Year != DateTime.Now.Year)
            {
                MonthCount = 0;
                MonthProcessCount = 0;
                lastMonthReset = DateTime.Now; // 마지막 월간 초기화 날짜 갱신
            }

            // ✔ 오늘 작업량 갱신
            TodayProcessCount = MachineProcessCount ?? 0;

            // DB에서 TodayProcessCount 값 가져오기
            TodayProcessCountFormat = MachineProcessCount.ToString() + " / 250";

            // WeekProcessCount와 MonthProcessCount는 각각 계속 누적
            WeekProcessCount = TodayProcessCount + WeekCount;
            WeekProcessCountFormat = MachineProcessCount.ToString() + " / 1250";

            MonthProcessCount = TodayProcessCount + MonthCount;
            MonthProcessCountFormat = MachineProcessCount.ToString() + " / 5000";
        }



        private void DateTimeAndGraphUpdate()
        {
            // 현재 시간을 구합니다.
            DateTime currentTime = DateTime.Now;

            // 시간과 분을 추출합니다.
            int currentHour = currentTime.Hour;
            int currentMinute = currentTime.Minute;

            // 시간 증가 및 레이블 업데이트
            //int timeIndex = 0;
            string label = $"{currentHour}시 {currentMinute}분";

            // UI 스레드에서 xLabel 업데이트
            Application.Current.Dispatcher.Invoke(() =>
            {
                XLabel.Add(label);

                // X축 레이블도 추가하고 갱신
                if (XLabel.Count > 8)
                    XLabel.RemoveAt(0);
            });

            // 그래프 
            // 그래프 업데이트
            Application.Current.Dispatcher.Invoke(() =>
            {
                // 온도 그래프 값 추가
                SeriesData[0].Values.Add(Mc.Temperature);
                if (SeriesData[0].Values.Count > 8)
                    SeriesData[0].Values.RemoveAt(0);

                // 습도 그래프 값 추가
                SeriesData[1].Values.Add(Mc.Humidity);
                if (SeriesData[1].Values.Count > 8)
                    SeriesData[1].Values.RemoveAt(0);

                // 오염도 그래프 값 추가
                SeriesData[2].Values.Add(Mc.Contamination_level);
                if (SeriesData[2].Values.Count > 8)
                    SeriesData[2].Values.RemoveAt(0);
            });
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

        [RelayCommand]
        private async void OpenDailyWindow()
        {
            MachineProcessCount = await machineStatusRepository.SelectTodayTotalCount();
            TodayProcessAmount();

            // 새 시간 값 반영
            DateTimeAndGraphUpdate2();

            var window = new ProcessGraph1();
            window.DataContext = this;
            window.Show();
        }

    //    public void DrawNewGraph()
    //    {
    //        NewXLabel = new ObservableCollection<string>();
            

    //        NewSeries = new SeriesCollection
    //{
    //    new LineSeries
    //    {
    //        Title = "오늘 작업량",
    //        Values = new ChartValues<int> { TodayProcessCount },
    //        Stroke = System.Windows.Media.Brushes.DeepSkyBlue,
    //        Fill = System.Windows.Media.Brushes.Transparent,
    //        PointGeometry = DefaultGeometries.Circle,
    //        StrokeThickness = 2
    //    },
    //    new LineSeries
    //    {
    //        Title = "전체 기준 작업량 (250)",
    //        Values = new ChartValues<double> { 250 },
    //        Stroke = System.Windows.Media.Brushes.Gray,
    //        Fill = System.Windows.Media.Brushes.Transparent,
    //        PointGeometry = null,
    //        StrokeThickness = 2,
    //        StrokeDashArray = new System.Windows.Media.DoubleCollection { 2 }
    //    }
    //};

    //        // 값 포맷터
    //        NewValuesFormatter = val => $"{val}";
    //    }

        private void DateTimeAndGraphUpdate2()
        {
            // 현재 시간 및 작업량 기록
            DateTime now = DateTime.Now;
            int currentCount = TodayProcessCount;

            // 새로운 데이터 추가
            _timeSeriesData.Add((now, currentCount));

            // 최대 8개만 유지
            if (_timeSeriesData.Count > 8)
                _timeSeriesData.RemoveAt(0);

            // 레이블과 그래프 값 갱신
            NewXLabel = new ObservableCollection<string>(
                _timeSeriesData.Select(t => t.Time.ToString("HH:mm"))
            );

            var todayValues = new ChartValues<int>(
                _timeSeriesData.Select(t => t.Count)
            );

            var fixedLine = new ChartValues<int>(Enumerable.Repeat(250, _timeSeriesData.Count));

            NewSeries = new SeriesCollection
    {
        new LineSeries
        {
            Title = "오늘 작업량",
            Values = todayValues,
            Stroke = System.Windows.Media.Brushes.DeepSkyBlue,
            Fill = System.Windows.Media.Brushes.Transparent,
            PointGeometry = DefaultGeometries.Circle,
            StrokeThickness = 2
        },
        new LineSeries
        {
            Title = "전체 기준 작업량",
            Values = fixedLine,
            Stroke = System.Windows.Media.Brushes.Gray,
            Fill = System.Windows.Media.Brushes.Transparent,
            PointGeometry = null,
            StrokeThickness = 2,
            StrokeDashArray = new System.Windows.Media.DoubleCollection { 2 }
        }
    };

            NewValuesFormatter = val => $"{val}";
        }

    }
}
