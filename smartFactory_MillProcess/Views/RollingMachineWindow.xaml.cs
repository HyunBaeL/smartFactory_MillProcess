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
    /// Interaction logic for RollingMachineWindow.xaml
    /// </summary>
    public partial class RollingMachineWindow : Window
    {
        // public RollingMachineViewModel RMVM;
        public RollingMachineWindow()
        {
            InitializeComponent();

            // RMVM = new RollingMachineViewModel();
            //DataContext = RMVM;


            //App.RollingVM.AverageTemperatureFromFurnace();
            App.RollingVM.SelectedMaterial = App.FurnaceVM.SelectedMaterial;
            DataContext = App.RollingVM;
        }

    }
}
