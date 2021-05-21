using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SamuraiApp.UI
{
    internal class Program
    {
        private static SamuraiContext _context = new SamuraiContext();

        private static void AddAllSamuraiToAllBattles()
        {
            var samurais = _context.Samurais.Where(w => w.Id != 9).ToList();
            var battles = _context.Battles.ToList();

            foreach (var battle in battles)
            {
                battle.Samurais.AddRange(samurais);
            }
            _context.SaveChanges();
        }

        private static void AddHorseToSamurai()
        {
            var samurai = _context.Samurais.Find(9);
            samurai.Horse = new Horse { Name = "Wolf" };
            _context.SaveChanges();
        }

        private static void AddNewSamuraiToExistingBattle()
        {
            var battle = _context.Battles.FirstOrDefault();
            battle.Samurais.Add(new Samurai { Name = "Grom Hellscream" });
            _context.SaveChanges();
        }

        private static void AddQuotesToExistingSamuraiWhileTracked()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Quotes.Add(new Quote { Text = "Poznaj magie mojego miecza" });
            _context.SaveChanges();
        }

        private static void AddQuotesToExistingSanuraiNotTracked(int SamuraiId)
        {
            var samurai = _context.Samurais.Find(SamuraiId);
            samurai.Quotes.Add(new Quote { Text = "Oddam życie za horde" });
            using (var newContext = new SamuraiContext())
            {
                //newContext.Samurais.Update(samurai);
                //attach nie updateuje samurai jest szybsze
                newContext.Samurais.Attach(samurai);
                newContext.SaveChanges();
            }
        }

        private static void AddSamuraiByName(params string[] names)
        {
            foreach (var name in names)
            {
                _context.Samurais.Add(new Samurai { Name = name });
            }
            _context.SaveChanges();
        }

        private static void AddSamuraiWithAQuote()
        {
            var samurai = new Samurai
            {
                Name = "Robert",
                Quotes = new List<Quote>
                {
                    new Quote{ Text = "Lok Tar Ogar"}
                }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void AddSamuraiWithManyQuote()
        {
            var samurai = new Samurai
            {
                Name = "Robert",
                Quotes = new List<Quote>
                {
                    new Quote{ Text = "Lok Tar Ogar"},
                    new Quote{ Text = "Praca praca"},
                }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void AddVariousTypes()
        {
            _context.Samurais.AddRange(
                new Samurai { Name = "Krzyś" },
                new Samurai { Name = "Puchatek" });
            _context.Battles.AddRange(
                new Battle { Name = "Helmowy Jar" },
                new Battle { Name = "Gondor" });
            _context.SaveChanges();
        }

        private static void Delete()
        {
            var samurai = _context.Samurais.Find(6);
            _context.Samurais.Remove(samurai);
            _context.SaveChanges();
        }

        private static void EagerLoadSamuraiWithQuotes()
        {
            // var samuraiWitjquotes = _context.Samurais.Include(s => s.Quotes).ToList();
            // var samuraiWitjquotes = _context.Samurais.AsSplitQuery().Include(s => s.Quotes).ToList();
            // var filteringQuotes = _context.Samurais.Include(s => s.Quotes.Where(q => q.Text == "Lok Tar Ogar")).ToList();
            var filteringQuotes2 = _context.Samurais.Where(w => w.Name == "Robert").Include(s => s.Quotes).ToList();
        }

        private static void ExpicitLoadQuotes()
        {
            _context.Set<Horse>().Add(new Horse { SamuraiId = 1, Name = "Wolf" });
            _context.SaveChanges();
            _context.ChangeTracker.Clear();

            var samurai = _context.Samurais.Find(1);
            _context.Entry(samurai).Collection(s => s.Quotes).Load();
            _context.Entry(samurai).Reference(s => s.Horse).Load();
        }

        private static void FilteringWithReletedData()
        {
            var samurai = _context.Samurais.Where(w => w.Quotes.Any(a => a.Text.Contains("Lok"))).ToList();
        }

        private static void GetSamurais(string text)
        {
            var samurais = _context.Samurais
                .TagWith("ConsoleApp.Program.GetSamurais method")
                .ToList();
            Console.WriteLine($"{text}: Samurai Count is {samurais.Count}");
            foreach (var samurai in samurais)
            {
                Console.WriteLine(samurai.Name);
            }
        }

        private static void Main(string[] args)
        {
            //_context.Database.EnsureCreated();
            //GetSamurais("Before: ");
            //AddSamuraiByName("Robert2", "Marta2");
            //AddVariousTypes();
            //GetSamurais("After: ");
            //QueryFilters();
            // Update();
            //UpdateMultiple();
            //Delete();
            //QuerryBattlesDisconected();
            // AddSamuraiWithAQuote();
            //AddSamuraiWithManyQuote();
            //AddSamuraiToExistingSauraiWhileTracked();
            // AddSamuraiToExistingSauraiNotTracked(1);
            // EagerLoadSamuraiWithQuotes();
            // ProjectSamuraiWithQuotes();
            // ExpicitLoadQuotes();
            // FilteringWithReletedData();
            // ModifyReltedDataWhenTracked();
            // ModifyReltedDataWhenNotTracked();
            // AddNewSamuraiToExistingBattle();
            //ReturnBattleWithSamurai();
            // AddAllSamuraiToAllBattles();

            //RemoveSamuraiFromBattle();
            //RemoveSamuraiFromBattleExpilicit();
            AddHorseToSamurai();
            Console.ReadKey();
        }

        private static void ModifyReltedDataWhenNotTracked()
        {
            var samurai = _context.Samurais.Include(s => s.Quotes).FirstOrDefault(f => f.Id == 1);
            var quote = samurai.Quotes[0];
            quote.Text = "Moje ostrze szuka zemsty";

            using var newcontext = new SamuraiContext();
            newcontext.Entry(quote).State = EntityState.Modified;
            newcontext.SaveChanges();
        }

        private static void ModifyReltedDataWhenTracked()
        {
            var samurai = _context.Samurais.Include(s => s.Quotes).FirstOrDefault(f => f.Id == 1);
            samurai.Quotes[0].Text = "Co mam dla ciebie zrobić";
            _context.SaveChanges();
        }

        private static void ProjectSamuraiWithQuotes()
        {
            //var somePropsWithQuotes = _context.Samurais.Select(s => new { s.Id, s.Name, NumberOfQuotes = s.Quotes.Count }).ToList();

            //var somePropsWithQuotes2 = _context.Samurais.Select(s => new { s.Id, s.Name, LokQuotes = s.Quotes.Where(q => q.Text.Contains("Lok")) }).ToList();

            var samuraiQuotes = _context.Samurais.Select(s => new { Samurai = s, LokQuotes = s.Quotes.Where(q => q.Text.Contains("Lok")) }).ToList();
            var firstSamurai = samuraiQuotes[0].Samurai.Name += "King";
        }

        private static void QuerryBattlesDisconected()
        {
            List<Battle> disconectedBattles;
            using (var context1 = new SamuraiContext())
            {
                disconectedBattles = context1.Battles.ToList();
            }
            disconectedBattles.ForEach(b =>
            {
                b.StartDate = new DateTime(2000, 01, 01);
                b.EmdDate = new DateTime(2000, 01, 02);
            });

            using (var context2 = new SamuraiContext())
            {
                context2.UpdateRange(disconectedBattles);
                context2.SaveChanges();
            }
        }

        private static void QueryFilters()
        {
            var samurais = _context.Samurais.Where(s => s.Name == "Robert").ToList();
            var contains = _context.Samurais.Where(s => s.Name.Contains("Rob")).ToList();
            //find is dbset method it can avoid database query
            var samurai2 = _context.Samurais.Find(2);
        }

        private static void RemoveSamuraiFromBattle()
        {
            var battleSamurai = _context.Battles.Include(b => b.Samurais.Where(w => w.Id == 9)).Single(s => s.BattleId == 1);
            var samurai = battleSamurai.Samurais[0];

            battleSamurai.Samurais.Remove(samurai);
            _context.SaveChanges();
        }

        private static void RemoveSamuraiFromBattleExpilicit()
        {
            var battleSamurai = _context.Set<BattleSamurai>().SingleOrDefault(s => s.BattleId == 1 && s.SamuraiId == 8);

            if (battleSamurai != null)
            {
                _context.Remove(battleSamurai);
                _context.SaveChanges();
            }
        }

        private static void ReturnBattleWithSamurai()
        {
            var battle = _context.Battles.Include(s => s.Samurais).FirstOrDefault();
            var battles = _context.Battles.Include(s => s.Samurais).ToList();
        }

        private static void Update()
        {
            var samurai = _context.Samurais.First();
            samurai.Name += "San";
            _context.SaveChanges();
        }

        private static void UpdateMultiple()
        {
            var samurai = _context.Samurais.Skip(1).Take(4).ToList();
            samurai.ForEach(s => s.Name += "San");
            _context.SaveChanges();
        }
    }
}