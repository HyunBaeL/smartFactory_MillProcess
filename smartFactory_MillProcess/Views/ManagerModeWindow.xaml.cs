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
using System.Linq;

namespace smartFactory_MillProcess.Views
{
    /// <summary>
    /// Interaction logic for ManagerMode.xaml
    /// </summary>
    public partial class ManagerModeWindow : Window
    {
        public EmployeeViewModel EmployeeVM { get; private set; }
        public ManagerModeViewModel ManagerModeVM { get; private set; }
        public MachineStatusViewModel MachineStatusVM { get; private set; }

        public ManagerModeWindow()
        {
            InitializeComponent();
            DataContext = new ManagerModeViewModel();


            // 사원 관리, 공정 현황, 요구 수량 변경
            ManagerModeVM = new ManagerModeViewModel(EmployeeVM, MachineStatusVM);
        }
        public Window Owner { get; internal set; }
    }
}
