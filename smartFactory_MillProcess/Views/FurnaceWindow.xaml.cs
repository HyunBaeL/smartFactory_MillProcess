using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using smartFactory_MillProcess.ViewModels;

namespace smartFactory_MillProcess.Views
{
    /// <summary>
    /// Interaction logic for FurnaceWindow.xaml
    /// </summary>
    public partial class FurnaceWindow : Window
    {
        public FurnaceViewModel viewModel;
        public FurnaceViewModel ViewModel { get; set; }
        public FurnaceWindow()
        {
            InitializeComponent();

            // 이벤트 처리 로직
            viewModel = new FurnaceViewModel
            {
                plotControl = PlotControl
            };

            this.DataContext = viewModel;
        }
    }
}
