using System.Configuration;
using System.Data;
using System.Windows;
using smartFactory_MillProcess.ViewModels;

namespace smartFactory_MillProcess
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static FurnaceViewModel FurnaceVM { get; } = new FurnaceViewModel();
        public static RollingMachineViewModel RollingVM { get; } = new RollingMachineViewModel();
    }

}
