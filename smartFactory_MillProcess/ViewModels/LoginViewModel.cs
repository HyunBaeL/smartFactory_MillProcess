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

        [ObservableProperty]
        private string id;

        [ObservableProperty]
        private string password;

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
        private void Login()
        {
            bool check = false;

            foreach (var u in Users)
            {
                if (u.Id.Equals(Id) && u.Pwd.Equals(Password))
                {
                    check = true;
                    MessageBox.Show($"{u.Name}님 로그인", "로그인", MessageBoxButton.OK, MessageBoxImage.Information);
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