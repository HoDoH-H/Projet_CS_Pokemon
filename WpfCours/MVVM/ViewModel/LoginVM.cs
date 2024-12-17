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
    public class LoginVM : BaseVM
    {
        public ICommand RequestChangeViewCommand { get; set; }
        public ICommand RequestChangeViewToRegisterCommand { get; set; }
        private string password;
        private string error;

        public string Username { get { return GlobalVars.Instance.Username; } set { if (SetProperty(ref GlobalVars.Instance.Username, value)){ OnPropertyChanged(nameof (Username)); } } }
        public string Password { get { return password; } set { if (SetProperty(ref password, value)){ OnPropertyChanged(nameof (Password)); } } }
        public string Error { get { return error; } set { if (SetProperty(ref error, value)){ OnPropertyChanged(nameof (Error)); } } }

        public LoginVM() 
        {
            RequestChangeViewCommand = new RelayCommand(HandleRequestChangeViewCommand);
            RequestChangeViewToRegisterCommand = new RelayCommand(HandleRequestChangeViewToRegisterCommand);
            password = string.Empty;
            error = string.Empty;
        }
        
        public void HandleRequestChangeViewCommand()
        {
            if (error != null)
                ShowErrorText();


            if (CheckLogin())
            {
                MainWindowVM.OnRequestVMChange?.Invoke(new MainViewVM());
            }
            else
            {
                ShowErrorText("Username or Password incorrect.");
            }
        }

        public void HandleRequestChangeViewToRegisterCommand()
        {
            Password = string.Empty;
            Error = string.Empty;

            MainWindowVM.OnRequestVMChange?.Invoke(new RegisterVM());
        }

        public void ShowErrorText(string text = "")
        {
            Error = text;
        }

        bool CheckLogin()
        {
            var context = new ExerciceMonsterContext();

            if (context.Logins.FirstOrDefault(x => x.Username == Username && x.PasswordHash == GlobalVars.Instance.HashString(Password)) != null)
                return true;

            return false;
        }

        public override void OnShowVM()
        {

        }
    }
}
