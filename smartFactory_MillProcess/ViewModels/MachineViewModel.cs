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

namespace smartFactory_MillProcess.ViewModels
{
    public partial class MachineViewModel : ObservableObject
    {
        private MachineRepository machineRepo = new MachineRepository();

        [ObservableProperty]
        private Machine mc;

        [ObservableProperty]
        private ObservableCollection<string> xLabel;
        public string[] XLabelArray => XLabel.ToArray();

        [ObservableProperty]
        public SeriesCollection seriesData;

        // ✔ 이름 명확하게 변경
        public Func<double, string> ValuesFormatter { get; set; }

        public MachineViewModel()
        {
            MachineInfo();

            seriesData = new SeriesCollection
        {
            new LineSeries
            {
                Title = "온도",
                Values = new ChartValues<double> { 2.5, 72.0, 12.0, 56.7, 30.2, 2.5, 72.0, 12.0, 56.7, 30.2, 2.5, 72.0, 12.0, 56.7, 30.2 },
                PointGeometry = DefaultGeometries.Circle
            },
            new LineSeries
            {
                Title = "습도",
                Values = new ChartValues<double> { 5.0, 77.0, 20.0, 60, 90, 5.0, 77.0, 20.0, 60 },
                PointGeometry = DefaultGeometries.Square
            },
            //new LineSeries
            //{
            //    Title = "오염도",
            //    Values = new ChartValues<double> { 350, 420, 250, 370, 90, 0, 400, 120, 470 },
            //    PointGeometry = DefaultGeometries.Square
            //}

        };

            xLabel = new ObservableCollection<string> { "8시", "9시", "10시", "11시", "12시", "13시", "14시", "15시", "16시", "17시", "18시", "19시", "20시", "21시", "22시" };

            // ✔ LabelFormatter 함수 정의
            ValuesFormatter = val => $"{val:F1}";
        }

        private async void MachineInfo()
        {
            Mc = await machineRepo.GetMachine();
        }
    }
}
