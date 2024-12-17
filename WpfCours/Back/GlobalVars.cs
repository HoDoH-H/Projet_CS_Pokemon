using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using WpfCours.Model;
using WpfCours.MVVM.ViewModel;

namespace WpfCours.Back
{
    internal class GlobalVars
    {
        public string Username;

        public static GlobalVars Instance;

        public string ServerName;

        public Monster SelectedMonster;
        public bool MonsterSelected = false;

        public BaseVM CurrentVM;

        static public GlobalVars GetInstance()
        {
            if (Instance == null)
                Instance = new GlobalVars();
            return Instance;
        }
        public GlobalVars()
        {
            Username = string.Empty;
            ServerName = "<YourServerName>\\SQLEXPRESS";

            if (Instance == null)
                Instance = this;
            else
                throw new Exception("You can't create a new Application object");
        }

        #region Database

        #region Database Init

        public bool IsMonstersInitiated()
        {
            var context = new ExerciceMonsterContext();
            return !context.Monsters.Any();
        }

        public void Init()
        {
            var context = new ExerciceMonsterContext();
            var tableNames = context.Model.GetEntityTypes().Select(t => t.GetTableName()).Distinct().ToList();
            foreach (var table in tableNames)
            {
                if (table.ToLower() != "login")
                    context.Database.ExecuteSqlRaw($"DELETE FROM [{table}]");
            }

            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Spell', RESEED,0);");
            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Monster', RESEED,0);");

            context.SaveChanges();

            #region Spells
            context.Spells.Add(new Spell() { Name = "Tackle", Damage = 10, Description = "A physical attack in which the user charges and slams into the target with its whole body." });
            context.Spells.Add(new Spell() { Name = "Vine Whip", Damage = 15, Description = "The target is struck with slender, whiplike vines to inflict damage." });
            context.Spells.Add(new Spell() { Name = "Ember", Damage = 20, Description = "The target is attacked with small flames." });
            context.Spells.Add(new Spell() { Name = "Water Gun", Damage = 18, Description = "The target is blasted with a forceful shot of water." });
            context.Spells.Add(new Spell() { Name = "Bug Bite", Damage = 20, Description = "The user bites the target." });
            context.Spells.Add(new Spell() { Name = "Gust", Damage = 10, Description = "Litteraly Tackle here..." });
            context.Spells.Add(new Spell() { Name = "Quick Attack", Damage = 10, Description = "Litteraly Tackle here..." });
            context.Spells.Add(new Spell() { Name = "Bite", Damage = 20, Description = "The target is bitten with viciously sharp fangs." });
            context.Spells.Add(new Spell() { Name = "Take Down", Damage = 35, Description = "A reckless, full-body charge attack for slamming into the target." });
            context.Spells.Add(new Spell() { Name = "Splash", Damage = 0, Description = "The user just flops and splashes around to no effect at all..." });
            #endregion Spells

            context.SaveChanges();

            #region Monsters
            context.Monsters.Add(new Monster() { Health = 45, Name = "Bulbasaur", ImageUrl = "https://img.pokemondb.net/sprites/black-white/normal/bulbasaur.png", Spells = { context.Spells.FirstOrDefault(x => x.Name == "Tackle"), context.Spells.FirstOrDefault(x => x.Name == "Vine Whip") } });
            context.Monsters.Add(new Monster() { Health = 39, Name = "Charmander", ImageUrl = "https://img.pokemondb.net/sprites/black-white/normal/charmander.png", Spells = { context.Spells.FirstOrDefault(x => x.Name == "Tackle"), context.Spells.FirstOrDefault(x => x.Name == "Ember") } });
            context.Monsters.Add(new Monster() { Health = 44, Name = "Squirtle", ImageUrl = "https://img.pokemondb.net/sprites/black-white/normal/squirtle.png", Spells = { context.Spells.FirstOrDefault(x => x.Name == "Tackle"), context.Spells.FirstOrDefault(x => x.Name == "Water Gun") } });
            context.Monsters.Add(new Monster() { Health = 45, Name = "Caterpie", ImageUrl = "https://img.pokemondb.net/sprites/black-white/normal/caterpie.png", Spells = { context.Spells.FirstOrDefault(x => x.Name == "Tackle"), context.Spells.FirstOrDefault(x => x.Name == "Bug Bite") } });
            context.Monsters.Add(new Monster() { Health = 40, Name = "Pidgey", ImageUrl = "https://img.pokemondb.net/sprites/black-white/normal/pidgey.png", Spells = { context.Spells.FirstOrDefault(x => x.Name == "Tackle"), context.Spells.FirstOrDefault(x => x.Name == "Gust"), context.Spells.FirstOrDefault(x => x.Name == "Quick Attack") } });
            context.Monsters.Add(new Monster() { Health = 40, Name = "Ratata", ImageUrl = "https://img.pokemondb.net/sprites/black-white/normal/rattata-f.png", Spells = { context.Spells.FirstOrDefault(x => x.Name == "Tackle"), context.Spells.FirstOrDefault(x => x.Name == "Quick Attack"), context.Spells.FirstOrDefault(x => x.Name == "Bite"), context.Spells.FirstOrDefault(x => x.Name == "Take Down") } });
            context.Monsters.Add(new Monster() { Health = 20, Name = "Magikarp", ImageUrl = "https://img.pokemondb.net/sprites/black-white/normal/magikarp.png", Spells = { context.Spells.FirstOrDefault(x => x.Name == "Splash") } });
            #endregion Monsters

            context.SaveChanges();
        }

        // Only used in debugging
        public void CleanDatabase()
        {
            var context = new ExerciceMonsterContext();
            var tableNames = context.Model.GetEntityTypes().Select(t => t.GetTableName()).Distinct().ToList();
            foreach (var table in tableNames)
            {
                context.Database.ExecuteSqlRaw($"DELETE FROM [{table}]");
            }

            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Spell', RESEED,0);");
            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Monster', RESEED,0);");

            context.SaveChanges();
        }

        #endregion Database Init

        public int GetNumberOfMonster()
        {
            var context = new ExerciceMonsterContext();
            return context.Monsters.Count();
        }

        public List<Monster> GetPokedex()
        {
            var context = new ExerciceMonsterContext();
            return context.Monsters.ToList();
        }

        public Monster GetMonsterWithName(string name)
        {
            var context = new ExerciceMonsterContext();
            return context.Monsters.FirstOrDefault(x => x.Name == name);
        }

        public Spell GetSpellWithName(string name)
        {
            var context = new ExerciceMonsterContext();
            return context.Spells.FirstOrDefault(x => x.Name == name);
        }

        public List<Spell> GetSpellFromMonster(Monster monster)
        {
            var context = new ExerciceMonsterContext();
            return context.Spells.Where(x => x.Monsters.FirstOrDefault(m => m.Name == monster.Name) != null).ToList();
        }

        public string HashString(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                StringBuilder hashString = new StringBuilder();
                foreach (var b in hashBytes)
                {
                    hashString.Append(b.ToString("x2")); // Convert to hexadecimal
                }

                return hashString.ToString();
            }
        }

        #endregion Database
    }
}
