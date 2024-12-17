using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfCours.Model;
using WpfCours.MVVM.View;
using WpfCours.Back;

namespace WpfCours.MVVM.ViewModel
{
    public class RegisterVM : BaseVM
    {
        public ICommand RequestChangeViewCommand { get; set; }
        public ICommand RequestChangeViewToLoginCommand { get; set; }
        private string password;
        private string error;

        public string Username { get { return GlobalVars.Instance.Username; } set { if (SetProperty(ref GlobalVars.Instance.Username, value)){ OnPropertyChanged(nameof (Username)); } } }
        public string Password { get { return password; } set { if (SetProperty(ref password, value)){ OnPropertyChanged(nameof (Password)); } } }
        public string Error { get { return error; } set { if (SetProperty(ref error, value)){ OnPropertyChanged(nameof (Error)); } } }

        public RegisterVM() 
        {
            RequestChangeViewCommand = new RelayCommand(HandleRequestChangeViewCommand);
            RequestChangeViewToLoginCommand = new RelayCommand(HandleRequestChangeViewToLoginCommand);
            password = string.Empty;
            error = string.Empty;
        }
        
        public void HandleRequestChangeViewCommand()
        {
            if (error != null)
                ShowErrorText();


            if (CheckRegister())
            {
                var context = new ExerciceMonsterContext();
                context.Logins.Add(new Login() { Username = GlobalVars.Instance.Username, PasswordHash = GlobalVars.Instance.HashString(Password) });
                context.SaveChanges();

                MainWindowVM.OnRequestVMChange?.Invoke(new MainViewVM());
            }
        }

        public void HandleRequestChangeViewToLoginCommand()
        {
            Password = string.Empty;
            Error = string.Empty;

            MainWindowVM.OnRequestVMChange?.Invoke(new LoginVM());
        }

        public void ShowErrorText(string text = "")
        {
            Error = text;
        }

        bool CheckRegister()
        {
            var context = new ExerciceMonsterContext();

            if (GlobalVars.Instance.Username == string.Empty)
            {
                ShowErrorText("Username can't be empty.");
                return false;
            }

            if (Password == string.Empty)
            {
                ShowErrorText("Password can't be empty.");
                return false;
            }

            if (context.Logins.FirstOrDefault(x => x.Username == Username) != null)
            {
                ShowErrorText("This username isn't available.");
                return false;
            }

            return true;
        }

        public override void OnShowVM()
        {

        }
    }
}
