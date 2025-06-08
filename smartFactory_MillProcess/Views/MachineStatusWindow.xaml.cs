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
    /// Interaction logic for MachineStatusWindow.xaml
    /// </summary>
    public partial class MachineStatusWindow : Window
    {
        public MachineStatusWindow()
        {
            InitializeComponent();
            DataContext = new MachineStatusViewModel();
        }
        public Window Owner { get; internal set; }
    }
}
