using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using smartFactory_MillProcess.Views;

namespace smartFactory_MillProcess.ViewModels
{
<<<<<<< Updated upstream
    public partial class FurnaceViewModels : Page
    {
        public bool IsMenuOpen { get; set; }
=======

    private Furnace furnaceModel = new Furnace();  // 🔹 Model 객체 생성
    private DispatcherTimer timer;  // 🔹 타이머 객체
    public bool IsMenuOpen { get; set; }

    private int elapsedSeconds;  // 🔹 경과 시간
    private Random random = new Random();  // 🔹 난수 생성기

    [ObservableProperty]
    private int displayTemperature;  // 🔹 UI에 자동 반영되는 속성 (사용자 입력값)

    [ObservableProperty]
    private int generatedTemperature;  // 🔹 UI에 자동 반영되는 속성 (난수 온도)

    [ObservableProperty]
    private double averageTemperature;  // 🔹 UI에 자동 반영되는 속성 (평균 온도) + app.xaml.cs에서 사용됨
    [ObservableProperty]
    private string userInput;
    [ObservableProperty]
    private double progressValue;
    [ObservableProperty]
    private string selectedMaterial;
    [ObservableProperty]
    private int minAllowedTemp = 0;

    [ObservableProperty]
    private int maxAllowedTemp = 0;

    [ObservableProperty]
    private double oxideScale;  // 산화 스케일

    //// 창이 닫힐 때 평균 온도 저장
    //public void OnWindowClosed()
    //{
    //    AverageTemperature = CalculateAverageTemperature();

    //    var rollingVM = new RollingMachineViewModel(AverageTemperature);
    //    var rollingMachineWindow = new RollingMachineWindow(AverageTemperature)
    //    {
    //        DataContext = rollingVM
    //    };
    //    rollingMachineWindow.Show();
    //}

    // 🔹 Model의 온도 기록 가져오기

    public WpfPlot plotControl;
    private List<double> temperatureHistory = new List<double>();  // ✅ 온도 기록 저장
    private List<double> timeHistory = new List<double>();
    public ObservableCollection<double> TemperatureHistory { get; }
    public ObservableCollection<double> TimeHistory { get; }
    

    public ObservableCollection<string> MaterialOptions { get; } = new ObservableCollection<string>
    {
        "Al5082",
        "SUS304",
        "SM45C"
    };


    public FurnaceViewModel()
    {    
        
        timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += UpdateTemperature;
        furnaceModel = new Furnace();
        TemperatureHistory = new ObservableCollection<double>(furnaceModel.TemperatureHistory);
        TimeHistory = new ObservableCollection<double>(furnaceModel.TimeHistory);
        
    }

    
    partial void OnSelectedMaterialChanged(string value)
    {
        // 재료 선택 시 온도 범위 변경 (예: 이 정보를 기준으로 UI 검증에 사용)
        switch (value)
        {
            case "Al5082":
                MinAllowedTemp = 900;
                MaxAllowedTemp = 1100;
                MessageBox.Show($"{MinAllowedTemp}~{MaxAllowedTemp} 사이의 값을 입력하세요");
                break;
            case "SUS304":
                MinAllowedTemp = 1100;
                MaxAllowedTemp = 1200;
                MessageBox.Show($"{MinAllowedTemp}~{MaxAllowedTemp} 사이의 값을 입력하세요");
                break;
            case "SM45C":
                MinAllowedTemp = 1200;
                MaxAllowedTemp = 1300;
                MessageBox.Show($"{MinAllowedTemp}~{MaxAllowedTemp} 사이의 값을 입력하세요");
                break;
            default:
                MinAllowedTemp = 0;
                MaxAllowedTemp = 0;
                MessageBox.Show("재료를 선택해주세요");
                break;
        }
    }

    


    [RelayCommand]
    private void StartTemperatureUpdate()
    {
        
        if (int.TryParse(UserInput, out int userTemperature) && userTemperature >= MinAllowedTemp && userTemperature <= MaxAllowedTemp)
        {
            if (timer.IsEnabled)
            {
                timer.Stop(); //  기존 타이머 중단
            }

            DisplayTemperature = userTemperature;

            //  타이머 및 기록 초기화
            elapsedSeconds = 0;
            furnaceModel.TemperatureHistory.Clear();
            furnaceModel.TimeHistory.Clear();

            ProgressValue = 0;
            AverageTemperature = 0;

            plotControl?.Plot.Clear();  //  그래프 초기화
            plotControl?.Refresh();

            timer.Start();
        }
        else
        {
            MessageBox.Show($"⚠ {MinAllowedTemp}~{MaxAllowedTemp}도 사이의 숫자를 입력하세요!");
        }
    }
    
    private void UpdateTemperature(object? sender, EventArgs e)
    {
        if (elapsedSeconds <= 60)
        {
            int X = CalculateX(DisplayTemperature);
            GeneratedTemperature = random.Next(DisplayTemperature - X, DisplayTemperature + X + 1);

            if (furnaceModel.TemperatureHistory.Count >= 60)
            {
                furnaceModel.TemperatureHistory.RemoveAt(0);
            }


            furnaceModel.TimeHistory.Add(elapsedSeconds);
            furnaceModel.TemperatureHistory.Add(GeneratedTemperature);
            ProgressValue = (elapsedSeconds * 100) / 60;
            elapsedSeconds++;
            UpdatePlot();


        }
        else
        {
            timer.Stop();
            AverageTemperature = CalculateAverageTemperature();

            oxideScale = CalcOxideScale(AverageTemperature, elapsedSeconds);
        }
    }
   
    public void UpdatePlot()
    {
        
        if (plotControl == null)
        {
            MessageBox.Show("plotControl이 초기화되지 않았습니다.");
            return;
        }


        var plt = plotControl.Plot;
        plt.Clear();
        plt.Add.Scatter(
            xs: furnaceModel.TimeHistory.ToArray(),
            ys: furnaceModel.TemperatureHistory.ToArray(),
            color: Colors.DarkOrange
        );
        plt.XLabel("Seconds(s)");
        plt.YLabel("Temperature(°C)");
        plt.Axes.SetLimitsX(0, 60);
        plt.Axes.SetLimitsY(MinAllowedTemp - 30, MaxAllowedTemp + 30);


        plotControl.Refresh();
    }

   
    private double CalculateAverageTemperature()
    {
        return furnaceModel.TemperatureHistory.Count > 0 ? furnaceModel.TemperatureHistory.Average() : 0;
    }

    private int CalculateX(int userTemperature)
    {
        return 10;
>>>>>>> Stashed changes
    }

    private Dictionary<string, (double k0, double Q)> FConst= new()
{
    { "Al5082", (1.2e-6, 137000) },   // 알루미늄
    { "SUS304", (8.5e-7, 172000) },   // 스테인리스
    { "SM45C",  (2.1e-6, 156000) }    // 탄소강
};
    private double CalcOxideScale(double averageTempCelsius, int holdingTimeSeconds)
    {
        double averageTempKelvin = averageTempCelsius + 273.15;

        if (FConst.TryGetValue(SelectedMaterial, out var constants))
        {
            double k0 = constants.k0;
            double Q = constants.Q;
            double rawvalue = k0 * Math.Exp(-Q / (8.314 * averageTempKelvin)) * Math.Sqrt(holdingTimeSeconds);

            OxideScale = rawvalue * 1e9;
        }

        return 0;
    }
}
