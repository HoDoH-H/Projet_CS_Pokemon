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
using WpfCours.MVVM.ViewModel;

namespace WpfCours.MVVM.View
{
    /// <summary>
    /// Logique d'interaction pour TabPage1View.xaml
    /// </summary>
    public partial class BattleView : UserControl
    {
        public BattleView()
        {
            InitializeComponent();
            BattleVM vm = (BattleVM)GlobalVars.Instance.CurrentVM;
            vm.UpdateAttackButtons += ShowAttackButtons;
            vm.SetOpponentSpriteVisibility += SetOpponentSpriteVisibility;
            vm.SetPlayerSpriteVisibility += SetPlayerSpriteVisibility;
            vm.SetEndBattleButtonsVisibility += ShowEndBattleButtons;
        }

        public void SetOpponentSpriteVisibility(bool isVisible)
        {
            if (isVisible)
            {
                OpponentSprite.Visibility = Visibility.Visible;
            }
            else
            {
                OpponentSprite.Visibility = Visibility.Hidden;
            }
        }

        public void SetPlayerSpriteVisibility(bool isVisible)
        {
            if (isVisible)
            {
                PlayerSprite.Visibility = Visibility.Visible;
            }
            else
            {
                PlayerSprite.Visibility = Visibility.Hidden;
            }
        }

        public void ShowAttackButtons(int n)
        {
            Attack0.Visibility = Visibility.Hidden;
            Attack1.Visibility = Visibility.Hidden;
            Attack2.Visibility = Visibility.Hidden;
            Attack3.Visibility = Visibility.Hidden;

            if (n >= 1)
            {
                Attack0.Visibility = Visibility.Visible;
            }
            if (n >= 2)
            {
                Attack1.Visibility = Visibility.Visible;
            }
            if (n >= 3)
            {
                Attack2.Visibility = Visibility.Visible;
            }
            if (n >= 4)
            {
                Attack3.Visibility = Visibility.Visible;
            }
        }

        public void ShowEndBattleButtons(bool visible)
        {
            QuitButton.Visibility = visible ? Visibility.Visible : Visibility.Hidden;
            ContinueButton.Visibility = visible ? Visibility.Visible : Visibility.Hidden;
        }
    }
}
