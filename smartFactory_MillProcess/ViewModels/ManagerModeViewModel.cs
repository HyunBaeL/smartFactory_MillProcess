using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using smartFactory_MillProcess.Models;
using smartFactory_MillProcess.Views;

namespace smartFactory_MillProcess.ViewModels
{
    public partial class ManagerModeViewModel : ObservableObject
    {
        public EmployeeViewModel EmployeeViewModel { get; set; } = new EmployeeViewModel();
        public MachineStatusViewModel MachineStatusViewModel { get; set; } = new MachineStatusViewModel();

        [ObservableProperty]
        private bool isMenuOpen;

        public ManagerModeViewModel()
        {
            EmployeeViewModel = new EmployeeViewModel();
            MachineStatusViewModel = new MachineStatusViewModel();
        }

        public ManagerModeViewModel(EmployeeViewModel employeeVM, MachineStatusViewModel machineStatusVM)
        {
            EmployeeViewModel = employeeVM;
            MachineStatusViewModel = machineStatusVM;
        }
        

        // 직원 관리 창으로 이동하는 커맨드
        [RelayCommand]
        private void MoveEmployeeManagement()
        {
            var employeeManagementWindow = new EmployeeManagementWindow();
            employeeManagementWindow.Show();
        }

        // 공정 현황 창으로 이동하는 커맨드
        [RelayCommand]
        private void MoveMachineStatus()
        {
            var machineStatusWindow = new MachineStatusWindow();
            machineStatusWindow.Show();
        }
    }
}
