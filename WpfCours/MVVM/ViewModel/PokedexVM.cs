using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WpfCours.Back;
using WpfCours.Model;
using WpfCours.MVVM.View;

namespace WpfCours.MVVM.ViewModel
{
    public class PokedexVM : BaseVM
    {
        List<Monster> pokedex;

        public Action<Button> AddNewPokedexEntry;
        public Action GoBack;
        public Action<Monster> GoToEntryDetails;
        public Action<Spell> GoToSpellDetails;

        public PokedexVM()
        {
            List<Monster> pokedex = new List<Monster>();
        }

        async void GeneratePokeEntries()
        {
            await Task.Delay(50);
            foreach (var m in pokedex)
            {
                Button btn = new Button
                {
                    Margin = new Thickness(5),
                    Width = 75,
                    Height = 75,
                    Tag = m.Name,
                };

                Image image = new Image
                {
                    Source = new BitmapImage(new System.Uri(m.ImageUrl)),
                    Width = 100,
                    Height = 100,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Stretch = System.Windows.Media.Stretch.Uniform,
                };

                btn.Content = image;

                btn.Click += WhenPokedexEntryClicked;

                AddNewPokedexEntry?.Invoke(btn);
            }
        }

        void WhenPokedexEntryClicked(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string pokemonName)
            {
                ShowPokemonDetails(pokemonName);
            }
        }

        public void WhenSpellClicked(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string spellName)
            {
                ShowSpellDetails(spellName);
            }
        }

        async void ShowPokemonDetails(string pokemonName)
        {
            Monster pokemon = GlobalVars.Instance.GetMonsterWithName(pokemonName);
            await Task.Delay(50);
            GoToEntryDetails?.Invoke(pokemon);
        }

        async void ShowSpellDetails(string spellName)
        {
            Spell spell = GlobalVars.Instance.GetSpellWithName(spellName);
            await Task.Delay(50);
            GoToSpellDetails?.Invoke(spell);
        }

        public override void OnShowVM()
        {
            pokedex = GlobalVars.Instance.GetPokedex();
            GeneratePokeEntries();
        }
    }
}
