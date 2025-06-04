using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using smartFactory_MillProcess.Repositories;
using smartFactory_MillProcess.Models;
using System.Windows;
using System.Collections.ObjectModel;
using smartFactory_MillProcess.Views;
using CommunityToolkit.Mvvm.Input;

namespace smartFactory_MillProcess.ViewModels
{
    public partial class EmployeeViewModel : ObservableObject
    {
        private EmployeeRepository employeeRepo = new EmployeeRepository();

        [ObservableProperty]
        private ObservableCollection<UserAndEmployeeInfo> emps = new ObservableCollection<UserAndEmployeeInfo>();

        public EmployeeViewModel()
        {
            AllEmployeeInfo();
        }

        private async Task AllEmployeeInfo()
        {
            try
            {
                List<UserAndEmployeeInfo> empList = await employeeRepo.GetEmployee();

                var filteredList = empList.Where(emp => emp.IsDelete == false);

                Emps = new ObservableCollection<UserAndEmployeeInfo>(filteredList);
            }
            catch (Exception ex) 
            {
                MessageBox.Show("사원 정보를 불러오지 못했습니다.");
                MainWindow.Instance.CanGoBack();
            }
        }

        [RelayCommand]
        private async Task UpdateEmployee()
        {
            var result = MessageBox.Show("정말 데이터를 변경하시겠습니까?", "데이터 변경", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                foreach (var emp in Emps)
                {
                    await employeeRepo.Update(emp);
                }

                await AllEmployeeInfo(); 
            }
        }


        [RelayCommand]
        private void OpenMemberAddDeleteWindow()
        {
            var popup = new MemberAddDeleteWindow
            {
                Owner = Application.Current.MainWindow
            };
            popup.Show();
        }
    }
}
