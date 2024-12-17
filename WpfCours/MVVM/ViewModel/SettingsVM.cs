using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfCours.Back;
using WpfCours.Model;
using WpfCours.MVVM.View;

namespace WpfCours.MVVM.ViewModel
{
    public class SettingsVM : BaseVM
    {
        public ICommand RequestChangeViewCommand { get; set; }
        public ICommand RequestResetCommand { get; set; }

        public SettingsVM()
        {
            RequestChangeViewCommand = new RelayCommand(HandleRequestChangeViewCommand);
            RequestResetCommand = new RelayCommand(HandleResetCommand);
        }

        void HandleRequestChangeViewCommand()
        {
            MainWindowVM.OnRequestVMChange?.Invoke(new MainViewVM());
        }

        async void HandleResetCommand()
        {
            GlobalVars.Instance.Init();
        }

        public override void OnShowVM()
        {

        }
    }
}
