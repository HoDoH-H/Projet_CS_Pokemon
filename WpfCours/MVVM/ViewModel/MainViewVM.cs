using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfCours.Back;
using WpfCours.Model;
using WpfCours.MVVM.View;

namespace WpfCours.MVVM.ViewModel
{
    public class MainViewVM : BaseVM
    {
        private string currentBg;

        //Called from view (With data binding)
        public ICommand RequestChangeViewCommand { get; set; }
        public ICommand RequestGoToPokedexCommand { get; set; }
        public ICommand RequestSettingsCommand { get; set; }
        public string CurrentBg { get { return currentBg; } set { if (SetProperty(ref currentBg, value)) { OnPropertyChanged(nameof(CurrentBg)); } } }

        public MainViewVM()
        {
            //Configure command to callback "HandleRequestChangeViewCommand"
            //when command is called
            RequestChangeViewCommand = new RelayCommand(HandleRequestChangeViewCommand);
            RequestGoToPokedexCommand = new RelayCommand(HandleRequestGoToPokedexCommand);
            RequestSettingsCommand = new RelayCommand(HandleRequestGoToSettingsCommand);
            currentBg = string.Empty;
        }

        public void HandleRequestChangeViewCommand()
        {
            MainWindowVM.OnRequestVMChange?.Invoke(new BattleVM());
        }

        public void HandleRequestGoToPokedexCommand()
        {
            MainWindowVM.OnRequestVMChange?.Invoke(new PokedexVM());
        }

        public void HandleRequestGoToSettingsCommand()
        {
            MainWindowVM.OnRequestVMChange?.Invoke(new SettingsVM());
        }

        //Override from BaseVM
        public override void OnShowVM()
        {
            currentBg = "https://c4.wallpaperflare.com/wallpaper/440/538/509/orange-fire-flame-caricature-fire-hd-wallpaper-preview.jpg";

            #region Init Database if needed

            if (GlobalVars.Instance.IsMonstersInitiated())
            {
                GlobalVars.Instance.Init();
            }

            if (!GlobalVars.Instance.MonsterSelected)
            {
                var context = new ExerciceMonsterContext();
                GlobalVars.Instance.SelectedMonster = context.Monsters.FirstOrDefault();
                GlobalVars.Instance.MonsterSelected = true;
            }

            #endregion Init Database if needed
        }
    }
}
