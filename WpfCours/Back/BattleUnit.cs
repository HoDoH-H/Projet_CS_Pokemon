using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfCours.Model;
using WpfCours.MVVM.ViewModel;

namespace WpfCours.Back
{
    public class BattleUnit
    {
        public bool IsPlayerUnit;

        public int MaxHealth;
        public int CurrentHealth;
        public int AttackBoost;

        public List<Spell> Spells;

        public Monster Monster { get; set; }

        public void DecreaseHP(int damage)
        {
            CurrentHealth -= damage;
            if (CurrentHealth < 0)
                CurrentHealth = 0;
        }

        public void IncreaseHP(int value)
        {
            CurrentHealth += value;
            if (CurrentHealth > MaxHealth)
                CurrentHealth = MaxHealth;
        }

        public void ResetStats()
        {
            MaxHealth = Monster.Health;
            AttackBoost = 0;
        }

        public float GetFormatHP()
        {
            return (0 + (CurrentHealth - 0) * (100 - 0) / (MaxHealth - 0));
        }
    }
}
