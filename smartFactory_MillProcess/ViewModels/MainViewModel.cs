using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using smartFactory_MillProcess.Views;

namespace smartFactory_MillProcess.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        public MachineViewModel MachineViewModel { get; } = new MachineViewModel();

        [ObservableProperty]
        private bool isMenuOpen;

        public MainViewModel()
        {

        }

        [RelayCommand]
        private void MoveProcessMainConductor()
        {
            MainWindow.Instance.Navigate(new ProcessMainConductor());
        }

        [RelayCommand]
        private void GoBack()
        {
            if (MainWindow.Instance.CanGoBack())
                MainWindow.Instance.GoBack();
        }

        [RelayCommand]
        private void Logout()
        {
            MainWindow.Instance.LoginVM.ClearIdAndPwd();
            MessageBox.Show("로그아웃", "로그아웃", MessageBoxButton.OK, MessageBoxImage.Information);
            MainWindow.Instance.Logout();
        }

        [RelayCommand]
        private void MoveEmployeeManagement()
        {
            MainWindow.Instance.Navigate(new EmployeeManagementPage());
        }

        [RelayCommand]
        private void Letsgo()
        {
            MainWindow.Instance.Navigate(new furnacePage());
        }
        [RelayCommand]
        private void Letsgo2()
        {
            MainWindow.Instance.Navigate(new RollingMachinePage());
        }
    }
}
