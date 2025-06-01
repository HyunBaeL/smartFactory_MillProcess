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


namespace smartFactory_MillProcess.ViewModels;

public partial class FurnaceViewModel : ObservableObject
{
    private Furnace furnaceModel = new Furnace();  // 🔹 Model 객체 생성
    private DispatcherTimer timer;  // 🔹 타이머 객체
      

    private int elapsedSeconds;  // 🔹 경과 시간
    private Random random = new Random();  // 🔹 난수 생성기

    [ObservableProperty]
    private int displayTemperature;  // 🔹 UI에 자동 반영되는 속성 (사용자 입력값)

    [ObservableProperty]
    private int generatedTemperature;  // 🔹 UI에 자동 반영되는 속성 (난수 온도)

    [ObservableProperty]
    private double averageTemperature;  // 🔹 UI에 자동 반영되는 속성 (평균 온도)
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

    // 🔹 Model의 온도 기록 가져오기

    public WpfPlot plotControl;
    private List<double> temperatureHistory = new List<double>();  // ✅ 온도 기록 저장
    private List<double> timeHistory = new List<double>();
    public ObservableCollection<double> TemperatureHistory { get; }
    public ObservableCollection<double> TimeHistory { get; }
    

    public ObservableCollection<string> MaterialOptions { get; } = new ObservableCollection<string>
    {
        "알루미늄",
        "스테인리스",
        "탄소강"
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
            case "알루미늄":
                MinAllowedTemp = 900;
                MaxAllowedTemp = 1100;
                break;
            case "스테인리스":
                MinAllowedTemp = 1100;
                MaxAllowedTemp = 1200;
                break;
            case "탄소강":
                MinAllowedTemp = 1200;
                MaxAllowedTemp = 1300;
                break;
            default:
                MinAllowedTemp = 1000;
                MaxAllowedTemp = 1300;
                break;
        }
    }

    


    [RelayCommand]
    private void StartTemperatureUpdate()
    {
        
        if (int.TryParse(UserInput, out int userTemperature) && userTemperature >= MinAllowedTemp && userTemperature <= MaxAllowedTemp)
        {
            DisplayTemperature = userTemperature;
            furnaceModel.TemperatureHistory.Clear();  // 🔹 Model의 온도 기록 초기화
            elapsedSeconds = 0;
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
    }
}







