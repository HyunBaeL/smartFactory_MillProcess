using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using smartFactory_MillProcess.Views;

namespace smartFactory_MillProcess.ViewModels
{
    public partial class MainConductorViewModel : ObservableObject
    {

        // 가열로 가동 버튼 구현
        [RelayCommand]
        private async Task FurnaceStatusView()
        {
            MessageBox.Show("가열로 가동");
            await Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var FurnaceWindow = new FurnaceWindow();
                    FurnaceWindow.Show();
                    //MainWindow.Instance.FurnaceVM.IsMenuOpen = true;
                });
            });
        }

        //// 가열로 프로그레스 바 완료 여부 확인 하여 UI 버튼 활성화 호출
        //[ObservableProperty]
        //private int furnacePercent;

        //public bool RunRollMachine => furnacePercent >= 100;

        //public MainConductorViewModel()
        //{
        //    // 진행률 속성이 바뀔 때마다 버튼 상태 업데이트하여 100%일때 커맨드 실행 가능하게 조정
        //    PropertyChanged += (_, e) =>
        //    {
        //        if (e.PropertyName == nameof(furnacePercent))
        //        {
        //            OnPropertyChanged(nameof(RunRollMachine));
        //        }
        //    };
        //}

        //[RelayCommand(CanExecute = nameof(RunRollMachine))]
        [RelayCommand]
        private async Task RollStatusView()
        {
            MessageBox.Show("압연기 가동");
            await Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {

                    var RollingMachinePage = new RollingMachinePage();
                    var furnacePage = new furnacePage();
                    // MainWindow.Instance.Navigate(RollingMachinePage);
                    // MainWindow.Instance.RollingMachineVM.IsMenuOpen = true;

                    var RollingMachineWindow = new RollingMachineWindow();
                    RollingMachineWindow.Show();
                    //MainWindow.Instance.RollingMachineVM.IsMenuOpen = true;

                });
            });
        }
    }
}
