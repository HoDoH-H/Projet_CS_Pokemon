using CommunityToolkit.Mvvm.Input;
using Microsoft.Data.SqlClient;
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
    public class DatabaseSetterVM : BaseVM
    {
        public ICommand RequestChangeViewCommand { get; set; }
        private string error;

        public string ServerName { get { return GlobalVars.Instance.ServerName; } set { if (SetProperty(ref GlobalVars.Instance.ServerName, value)){ OnPropertyChanged(nameof (ServerName)); } } }
        public string Error { get { return error; } set { if (SetProperty(ref error, value)){ OnPropertyChanged(nameof (Error)); } } }

        public DatabaseSetterVM() 
        {
            RequestChangeViewCommand = new RelayCommand(HandleRequestChangeViewCommand);
            error = string.Empty;
            ServerName = "<ServerName>\\SQLEXPRESS";
        }
        
        public async void HandleRequestChangeViewCommand()
        {
            if (error != null)
                ShowErrorText();

            // There was supposed to be a bool to check if the string is valid or not
            // but I couldn't find how to do that so if a wrong string is entered, the whole
            // program crashes.
            if (true)
            {
                MainWindowVM.OnRequestVMChange?.Invoke(new LoginVM());
            }
            else
            {
                ShowErrorText("Couldn't find any valid database.");
            }
        }

        public void ShowErrorText(string text = "")
        {
            Error = text;
        }

        public override void OnShowVM()
        {

        }
    }
}
