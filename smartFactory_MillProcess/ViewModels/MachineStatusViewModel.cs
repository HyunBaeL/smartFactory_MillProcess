using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MySql.Data.MySqlClient;
using smartFactory_MillProcess.Models;
using smartFactory_MillProcess.Repositories;
using smartFactory_MillProcess.Views;

namespace smartFactory_MillProcess.ViewModels
{
    public partial class MachineStatusViewModel : ObservableObject
    {
        private MachineStatusRepository machineStatusRepo = new MachineStatusRepository();

        [ObservableProperty]
        private ObservableCollection<MachineStatus> results = new ObservableCollection<MachineStatus>();

        [ObservableProperty]
        private int statusInfo;


        public MachineStatusViewModel()
        {
            AllStatusInfo();
        }

        private async Task AllStatusInfo()
        {
            StatusInfo = await machineStatusRepo.SelectTodayTotalCount();

            var dataList = await machineStatusRepo.SelectTodayResults();

            Results.Clear();
            foreach (var item in dataList)
            {
                Results.Add(item);
            }
        }
    }
}
