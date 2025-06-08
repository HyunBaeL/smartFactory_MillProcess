using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using smartFactory_MillProcess.Views;

namespace smartFactory_MillProcess.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        public MachineViewModel MachineViewModel { get; } = new MachineViewModel();
        public LoginViewModel LoginViewModel { get; set; } = new LoginViewModel();
        public RollingMachineViewModel RollingMachineViewModel { get; set;} = new RollingMachineViewModel();
        public ManagerModeViewModel ManagerModeViewModel { get; set; } = new ManagerModeViewModel();

        [ObservableProperty]
        private bool isMenuOpen;

        public MainViewModel()
        {

        }

        public MainViewModel(MachineViewModel machineVM)
        {
            MachineViewModel = machineVM;
        }

        public MainViewModel(LoginViewModel loginVM, ManagerModeViewModel ManagerModeVM, RollingMachineViewModel rollingVM)
        {
            LoginViewModel = loginVM;
            ManagerModeViewModel = ManagerModeVM; 
            RollingMachineViewModel = rollingVM;
        }

        [RelayCommand]
        private void MoveProcessMainConductor()
        {
            MainWindow.Instance.Navigate(new ProcessMainConductor());
        }

        [RelayCommand]
        private void MoveMainDashboard()
        {
            MainWindow.Instance.Navigate(new MainPage(this));
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
        private void MoveManagerMode()
        {
            var ManagerModeWindow = new ManagerModeWindow();
            ManagerModeWindow.Show();
        }

        [RelayCommand]
        private void CloseWindow()
        {
            Application.Current.Shutdown(); // 앱 종료 (MainWindow 닫기)
        }

        [RelayCommand]
        private void MinimizeWindow()
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        [RelayCommand]
        private void MaximizeRestoreWindow()
        {
            var window = Application.Current.MainWindow;
            window.WindowState = window.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
        }

        [RelayCommand]
        private void Letsgo()
        {
            var FurnaceWindow = new FurnaceWindow();
            FurnaceWindow.Show();
        }

        [RelayCommand]
        private void Letsgo2()
        {
            var RollingMachineWindow = new RollingMachineWindow();
            RollingMachineWindow.Show();
        }

    }
}
