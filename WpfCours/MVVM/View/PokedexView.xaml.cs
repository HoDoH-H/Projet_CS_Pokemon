using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfCours.Back;
using WpfCours.Model;
using WpfCours.MVVM.ViewModel;

namespace WpfCours.MVVM.View
{
    /// <summary>
    /// Logique d'interaction pour GameView.xaml
    /// </summary>
    public partial class PokedexView : UserControl
    {
        PokedexState state;
        Monster monster;

        Button[] attackButtons;

        PokedexVM vm;

        public PokedexView()
        {
            InitializeComponent();
            state = PokedexState.EntryList;
            attackButtons = new Button[] { Attack0, Attack1, Attack2, Attack3 };

            PokedexVM vm = (PokedexVM)GlobalVars.Instance.CurrentVM;
            this.vm = vm;

            vm.AddNewPokedexEntry += AddPokedexEntry;
            vm.GoToEntryDetails += GoToDetails;
            vm.GoBack += GoBack;
            vm.GoToSpellDetails += GoToSpellDetails;

            EntryList.Visibility = Visibility.Visible;
            EntryDetails.Visibility = Visibility.Collapsed;
            SpellDetails.Visibility = Visibility.Collapsed;
        }

        void AddPokedexEntry(Button button)
        {
            ButtonContainer.Children.Add(button);
        }

        async void GoToDetails(Monster monster)
        {
            state = PokedexState.EntryDetails;
            this.monster = monster;

            EntryList.Visibility = Visibility.Collapsed;
            SpellDetails.Visibility = Visibility.Collapsed;

            if (monster.Name == GlobalVars.Instance.SelectedMonster.Name)
            {
                SelectButton.Visibility = Visibility.Collapsed;
                CheckedImage.Visibility = Visibility.Visible;
            }
            else
            {
                SelectButton.Visibility = Visibility.Visible;
                CheckedImage.Visibility = Visibility.Collapsed;
            }

            Pokedex_DisplayPicture.Source = new BitmapImage(new System.Uri(monster.ImageUrl));
            Pokedex_Name.Text = $"Entry Name: {monster.Name}";
            Pokedex_Health.Text = $"Base Health: {monster.Health}";

            var spells = GlobalVars.Instance.GetSpellFromMonster(monster);
            await Task.Delay(50);

            for (int i = 0; i < attackButtons.Count(); i++)
            {
                if (i < spells.Count)
                {
                    attackButtons[i].Content = spells[i].Name;
                    attackButtons[i].Tag = spells[i].Name;
                }
                    
            }

            ShowAttackButtons(spells.Count);

            EntryDetails.Visibility = Visibility.Visible;
        }

        async void GoToSpellDetails(Spell spell)
        {
            state = PokedexState.SpellDetails;
            EntryList.Visibility = Visibility.Collapsed;
            EntryDetails.Visibility = Visibility.Collapsed;

            Spell_Name.Text = $"Spell Name: {spell.Name}";
            Spell_Desc.Text = $"Description:                            {spell.Description}";
            Spell_Damage.Text = $"Base Damage: {spell.Damage}";

            SpellDetails.Visibility = Visibility.Visible;
        }

        public void ShowAttackButtons(int n)
        {
            for (int i = 0; i < attackButtons.Count(); i++)
            {
                if (i < n)
                    attackButtons[i].Visibility = Visibility.Visible;
                else
                    attackButtons[i].Visibility = Visibility.Collapsed;
            }
        }

        void GoToList()
        {
            state = PokedexState.EntryList;
            EntryDetails.Visibility= Visibility.Collapsed;
            EntryList.Visibility= Visibility.Visible;
        }

        void GoBack()
        {
            if (state == PokedexState.EntryList)
            {
                MainWindowVM.OnRequestVMChange?.Invoke(new MainViewVM());
                return;
            }
            else if (state == PokedexState.EntryDetails)
            {
                GoToList();
                return;
            }
            else if (state == PokedexState.SpellDetails)
            {
                GoToDetails(monster);
                return;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GlobalVars.Instance.SelectedMonster = monster;
            SelectButton.Visibility = Visibility.Collapsed;
            CheckedImage.Visibility = Visibility.Visible;
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            GoBack();
        }

        private void SpellClicked(object sender, RoutedEventArgs e)
        {
            vm.WhenSpellClicked(sender, e);
        }
    }

    public enum PokedexState
    {
        EntryList,
        EntryDetails,
        SpellDetails,
    }
}
