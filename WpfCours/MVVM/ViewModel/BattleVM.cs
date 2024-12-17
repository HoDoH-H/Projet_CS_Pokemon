using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using WpfCours.Back;
using WpfCours.Model;
using WpfCours.MVVM.View;

namespace WpfCours.MVVM.ViewModel
{
    public class BattleVM : BaseVM
    {
        private int currentWave;
        private string wave;
        public string CurrentWave { get { return wave; } set { if (SetProperty(ref wave, value)) { OnPropertyChanged(nameof(CurrentWave)); } } }

        public Action<int> UpdateAttackButtons;
        public Action<bool> SetOpponentSpriteVisibility;
        public Action<bool> SetPlayerSpriteVisibility;
        public Action<bool> SetEndBattleButtonsVisibility;

        public BattleUnit playerUnit = new BattleUnit() { IsPlayerUnit = true};
        public BattleUnit opponentUnit = new BattleUnit() { IsPlayerUnit = false };

        private string attack0;
        private string attack1;
        private string attack2;
        private string attack3;
        private string consoleText;

        private string playerUnitSpriteURL;
        private string playerUnitName;
        private float formatizedPlayerHP;


        private string opponentUnitSpriteURL;
        private string opponentUnitName;
        private float formatizedOpponentHP;

        public string PlayerUnitName { get { return playerUnitName; } set { if (SetProperty(ref playerUnitName, value)) { OnPropertyChanged(nameof (PlayerUnitName)); } } }
        public string OpponentUnitName { get { return opponentUnitName; } set { if (SetProperty(ref opponentUnitName, value)) { OnPropertyChanged(nameof (OpponentUnitName)); } } }

        public string PlayerSpriteURL { get { return playerUnitSpriteURL; } set { if (SetProperty(ref playerUnitSpriteURL, value)){ OnPropertyChanged(nameof (PlayerSpriteURL)); } } }
        public float FormatizedPlayerHP { get { return formatizedPlayerHP; } set { if (SetProperty(ref formatizedPlayerHP, value)){ OnPropertyChanged(nameof (FormatizedPlayerHP)); } } }
        public float FormatizedOpponentHP { get { return formatizedOpponentHP; } set { if (SetProperty(ref formatizedOpponentHP, value)){ OnPropertyChanged(nameof (FormatizedOpponentHP)); } } }
        public string OpponentSpriteURL { get { return opponentUnitSpriteURL; } set { if (SetProperty(ref opponentUnitSpriteURL, value)){ OnPropertyChanged(nameof (OpponentSpriteURL)); } } }
        public string Attack0 { get { return attack0; } set { if (SetProperty(ref attack0, value)){ OnPropertyChanged(nameof (Attack0)); } } }
        public string Attack1 { get { return attack1; } set { if (SetProperty(ref attack1, value)){ OnPropertyChanged(nameof (Attack1)); } } }
        public string Attack2 { get { return attack2; } set { if (SetProperty(ref attack2, value)){ OnPropertyChanged(nameof (Attack2)); } } }
        public string Attack3 { get { return attack3; } set { if (SetProperty(ref attack3, value)){ OnPropertyChanged(nameof (Attack3)); } } }
        public string ConsoleText { get { return consoleText; } set { if (SetProperty(ref consoleText, value)){ OnPropertyChanged(nameof (ConsoleText)); } } }

        public ICommand Attack0Pressed { get; set; }
        public ICommand Attack1Pressed { get; set; }
        public ICommand Attack2Pressed { get; set; }
        public ICommand Attack3Pressed { get; set; }
        public ICommand QuitButtonPressed { get; set; }
        public ICommand ContinueButtonPressed { get; set; }

        public BattleVM() 
        {
            playerUnitSpriteURL = string.Empty;
            opponentUnitSpriteURL = string.Empty;

            attack0 = string.Empty;
            attack1 = string.Empty;
            attack2 = string.Empty;
            attack3 = string.Empty;

            Attack0Pressed = new RelayCommand(RelayAttack0);
            Attack1Pressed = new RelayCommand(RelayAttack1);
            Attack2Pressed = new RelayCommand(RelayAttack2);
            Attack3Pressed = new RelayCommand(RelayAttack3);
            QuitButtonPressed = new RelayCommand(RelayQuit);
            ContinueButtonPressed = new RelayCommand(RelayContinue);

            currentWave = 1;
            CurrentWave = $"Wave: 1";
        }
        
        public async void Setup()
        {
            SetEndBattleButtonsVisibility?.Invoke(false);

            var context = new ExerciceMonsterContext();
            Random random = new Random();
            int i = random.Next(0, GlobalVars.Instance.GetNumberOfMonster());

            playerUnit.Monster = GlobalVars.Instance.SelectedMonster;
            opponentUnit.Monster = context.Monsters.Skip(i).FirstOrDefault();

            playerUnitName = playerUnit.Monster.Name;
            OpponentUnitName = $"Wild {opponentUnit.Monster.Name}";

            CurrentWave = "Wave: 1";

            playerUnit.ResetStats();
            opponentUnit.ResetStats();

            PlayerSpriteURL = playerUnit.Monster.ImageUrl;
            OpponentSpriteURL = opponentUnit.Monster.ImageUrl;

            opponentUnit.Spells = GlobalVars.Instance.GetSpellFromMonster(opponentUnit.Monster);
            playerUnit.Spells = GlobalVars.Instance.GetSpellFromMonster(playerUnit.Monster);
            await Task.Delay(50);

            UpdateSpellUI(playerUnit.Spells.Count());

            playerUnit.MaxHealth = playerUnit.Monster.Health;
            playerUnit.CurrentHealth = playerUnit.MaxHealth;

            opponentUnit.MaxHealth = opponentUnit.Monster.Health;
            opponentUnit.CurrentHealth = opponentUnit.MaxHealth;

            UpdateHPs();
            PrintText($"What will {playerUnit.Monster.Name} do?");
        }

        public void UpdateSpellUI(int n = 0)
        {
            if (n == 0)
            {
                // Used to hide buttons when needed
                UpdateAttackButtons?.Invoke(0);
                return;
            }

            UpdateAttackButtons?.Invoke(playerUnit.Spells.Count());

            if (playerUnit.Spells.Count >= 1)
                Attack0 = playerUnit.Spells[0].Name;
            if (playerUnit.Spells.Count >= 2)
                Attack1 = playerUnit.Spells[1].Name;
            if (playerUnit.Spells.Count >= 3)
                Attack2 = playerUnit.Spells[2].Name;
            if (playerUnit.Spells.Count >= 4)
                Attack3 = playerUnit.Spells[3].Name;
        }

        void UpdateHPs()
        {
            FormatizedPlayerHP = playerUnit.GetFormatHP();
            FormatizedOpponentHP = opponentUnit.GetFormatHP();
        }

        public async void SoftSetup()
        {
            currentWave++;
            CurrentWave = $"Wave: {currentWave}";

            var context = new ExerciceMonsterContext();
            Random random = new Random();
            int i = random.Next(0, GlobalVars.Instance.GetNumberOfMonster());

            opponentUnit.Monster = context.Monsters.Skip(i).FirstOrDefault();

            PrintText($"A wild {opponentUnit.Monster.Name} appeared!");
            await Task.Delay(1000);

            OpponentSpriteURL = opponentUnit.Monster.ImageUrl;
            OpponentUnitName = $"Wild {opponentUnit.Monster.Name}";
            opponentUnit.Spells = GlobalVars.Instance.GetSpellFromMonster(opponentUnit.Monster);
            SetOpponentSpriteVisibility?.Invoke(true);

            AddStatBoost(opponentUnit);

            UpdateHPs();
            UpdateSpellUI(playerUnit.Spells.Count());
            PrintText($"What will {playerUnit.Monster.Name} do?");
        }

        public async void AddStatBoost(BattleUnit unit)
        {
            Random random = new Random();
            List<int> buffQuality = new List<int>();
            List<int> buffType = new List<int>();
            bool potionDiscovered = false;

            int[] healthBoost = unit.IsPlayerUnit ? [ 5, 10, 18, 25 ] : [ 2, 5, 10, 15 ];
            int[] damageBoost = unit.IsPlayerUnit ? [2, 5, 9, 14] : [1, 2, 4, 8];
            string[] buffQualityString = { "a commun", "a rare", "an epic", "a legendary" };

            if (unit.IsPlayerUnit)
            {
                // If unit is playerUnit
                buffType.Add(random.Next(2));
                buffQuality.Add(random.Next(100));
                potionDiscovered = random.Next(2) == 1 ? true : false;
            }
            else
            {
                // If unit is OpponentUnit

                // Reset stat boosts so every round will have enemies with random stats
                unit.MaxHealth = unit.Monster.Health;
                unit.AttackBoost = 0;

                for (int i = 0; i < currentWave; i++)
                {
                    buffType.Add(random.Next(2));
                    buffQuality.Add(random.Next(100));
                }
            }

            for (int i = 0; i < buffType.Count; i++)
            {
                int formatQuality = buffQuality[i] <= 54 ? 0 : buffQuality[i] <= 74 && buffQuality[i] > 54 ? 1 : buffQuality[i] <= 94 && buffQuality[i] > 74 ? 2 : 3;

                if (buffType[i] == 0)
                {
                    // Add Max Health
                    unit.MaxHealth += healthBoost[formatQuality];
                    unit.IncreaseHP(healthBoost[formatQuality]);
                    if (unit.IsPlayerUnit)
                        PrintText($"You found {buffQualityString[formatQuality]} HP Up (+ {healthBoost[formatQuality]} MaxHP)", true);
                }
                else if (buffType[i] == 1)
                {
                    // Add Damage Boost
                    unit.AttackBoost += damageBoost[formatQuality];
                    if (unit.IsPlayerUnit)
                        PrintText($"You found {buffQualityString[formatQuality]} Protein (+ {healthBoost[formatQuality]} Damage)", true);
                }
            }

            if (unit.IsPlayerUnit)
            {
                // If potion found then recover half of max hp
                if (potionDiscovered)
                {
                    unit.IncreaseHP(unit.MaxHealth / 2);
                    PrintText($"You found a potion! (+ {unit.MaxHealth / 2}HP)", true);
                }
            }
            else
            {
                // If opponent unit then recover hp to max hp
                unit.CurrentHealth = unit.MaxHealth;
            }

            UpdateHPs();
        }

        public void PrintText(string message, bool separateWindow = false)
        {
            if (!separateWindow)
            {
                ConsoleText = message;
            }
            else
            {
                MessageBox.Show(message);
            }
        }

        public async void RunTurn(Spell playerSpell)
        {
            UpdateSpellUI(0);

            Random r = new Random();
            int n = r.Next(3);

            if (n > 1)
            {
                // Opponent turn (1/3)
                RunMove(opponentUnit, opponentUnit.Spells[r.Next(opponentUnit.Spells.Count())], playerUnit);
                if (CheckIfBattleOver(playerUnit))
                {
                    SetPlayerSpriteVisibility?.Invoke(false);
                    await Task.Delay(1000);
                    RelayQuit();
                    return;
                }
                await Task.Delay(500);
                RunMove(playerUnit, playerSpell, opponentUnit);
                if (CheckIfBattleOver(opponentUnit))
                {
                    SetOpponentSpriteVisibility?.Invoke(false);
                    AddStatBoost(playerUnit);
                    await Task.Delay(500);
                    SetEndBattleButtonsVisibility?.Invoke(true);
                    return;
                }
            }
            else
            {
                // Player turn (2/3)
                RunMove(playerUnit, playerSpell, opponentUnit);
                if (CheckIfBattleOver(opponentUnit))
                {
                    SetOpponentSpriteVisibility?.Invoke(false);
                    AddStatBoost(playerUnit);
                    await Task.Delay(500);
                    SetEndBattleButtonsVisibility?.Invoke(true);
                    return;
                }
                await Task.Delay(500);
                RunMove(opponentUnit, opponentUnit.Spells[r.Next(opponentUnit.Spells.Count())], playerUnit);
                if (CheckIfBattleOver(playerUnit))
                {
                    SetPlayerSpriteVisibility?.Invoke(false);
                    await Task.Delay(1000);
                    RelayQuit();
                    return;
                }
            }

            await Task.Delay(1000);
            UpdateSpellUI(playerUnit.Spells.Count());
            PrintText($"What will {playerUnit.Monster.Name} do?");
        }

        public void RunMove(BattleUnit source, Spell spell, BattleUnit target)
        {
            var damage = spell.Damage + source.AttackBoost;
            target.DecreaseHP(damage);
            UpdateHPs();
            // TODO - Put a console where it say what attack the source unit uses.
            if (source.IsPlayerUnit)
                PrintText($"{source.Monster.Name} uses {spell.Name}");
            else
                PrintText($"Wild {source.Monster.Name} uses {spell.Name}");
        }

        public bool CheckIfBattleOver(BattleUnit unit)
        {
            if (unit.CurrentHealth <= 0)
                return true;
            
            return false;
        }

        public override void OnShowVM()
        {
            Setup();
        }

        #region Attack Buttons

        void RelayAttack0()
        {
            RunTurn(playerUnit.Spells[0]);
        }

        void RelayAttack1()
        {
            RunTurn(playerUnit.Spells[1]);
        }

        void RelayAttack2()
        {
            RunTurn(playerUnit.Spells[2]);
        }

        void RelayAttack3()
        {
            RunTurn(playerUnit.Spells[3]);
        }

        #endregion Attack Buttons

        void RelayQuit()
        {
            MainWindowVM.OnRequestVMChange?.Invoke(new MainViewVM());
        }
        async void RelayContinue()
        {
            SetEndBattleButtonsVisibility?.Invoke(false);
            await Task.Delay(500);
            SoftSetup();
        }
    }
}
