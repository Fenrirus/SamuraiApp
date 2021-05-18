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

        private static void AddSamuraiByName(params string[] names)
        {
            foreach (var name in names)
            {
                _context.Samurais.Add(new Samurai { Name = name });
            }
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
            QuerryBattlesDisconected();
            Console.ReadKey();
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