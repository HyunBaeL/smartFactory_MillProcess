using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using smartFactory_MillProcess.Repositories;
using smartFactory_MillProcess.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using smartFactory_MillProcess.Views;

namespace smartFactory_MillProcess.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private UserRepository userRepo = new UserRepository();
        private EmployeeRepository empRepo = new EmployeeRepository();

        [ObservableProperty]
        private string id;

        [ObservableProperty]
        private string name;
        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private int employeeId;

        [ObservableProperty]
        private string department;
        [ObservableProperty]
        private string position;

        [ObservableProperty]
        private ObservableCollection<Users> users = new ObservableCollection<Users>();

        public LoginViewModel()
        {
            UsersInfo();

        }

        private async void UsersInfo()
        {
            List<Users> userList = await userRepo.GetUsersAsync();
            Users = new ObservableCollection<Users>(userList);
        }

        [RelayCommand]
        private async Task Login()
        {
            bool check = false;

            foreach (var u in Users)
            {
                if (u.Id.Equals(Id) && u.Pwd.Equals(Password))
                {
                    check = true;
                    Name = u.Name;


                    Employee employee = new Employee();

                    employee = await empRepo.selectOne(u.EmployeeId);
                    EmployeeId = employee.EmployeeId;
                    Department = employee.Department;
                    Position = employee.Position;

                    MessageBox.Show($"{u.Name}님 환영합니다", "로그인", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainWindow.Instance.Navigate(new MainPage());
                    MainWindow.Instance.MainVM.IsMenuOpen = true;
                    break;
                }
            }

            if (!check)
            {
                MessageBox.Show("로그인 실패", "로그인", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void ClearIdAndPwd()
        {
            Id = string.Empty;
            Password = string.Empty;
        }

    }
}