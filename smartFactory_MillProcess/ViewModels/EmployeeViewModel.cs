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

namespace smartFactory_MillProcess.ViewModels
{
    public partial class EmployeeViewModel : ObservableObject
    {
        private EmployeeRepository employeeRepo = new EmployeeRepository();

        //[ObservableProperty]
        //private List<Employee> emp;

        [ObservableProperty]
        private ObservableCollection<Employee> emps = new ObservableCollection<Employee>();

        public EmployeeViewModel()
        {
            AllEmployeeInfo();
        }

        private async Task AllEmployeeInfo()
        {
            try
            {
                List<Employee> empList = await employeeRepo.GetEmployee();
                Emps = new ObservableCollection<Employee>(empList);
            }
            catch (Exception ex) 
            {
                MessageBox.Show("사원 정보를 불러오지 못했습니다.");
                MainWindow.Instance.CanGoBack();
            }
            

        }
    }
}
