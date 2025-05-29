using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using smartFactory_MillProcess.Repositories;
using smartFactory_MillProcess.Models;
using CommunityToolkit.Mvvm.Input;

namespace smartFactory_MillProcess.ViewModels
{
    public partial class MachineViewModel : ObservableObject
    {
        private MachineRepository machineRepo = new MachineRepository();

        [ObservableProperty]
        private Machine mc;

        public MachineViewModel()
        {
            MachineInfo();
        }

        private async void MachineInfo()
        {
            Mc = await machineRepo.GetMachine();
           
        }

        //[RelayCommand]
        //private void machineObject()
        //{

        //}
    }
}
