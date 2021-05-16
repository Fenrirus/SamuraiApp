using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Linq;

namespace SamuraiApp.UI
{
    internal class Program
    {
        private static SamuraiContext _context = new SamuraiContext();

        private static void AddSamurai()
        {
            var samurai = new Samurai { Name = "Robert" };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void GetSamurais(string text)
        {
            var samurais = _context.Samurais.ToList();
            Console.WriteLine($"{text}: Samurai Count is {samurais.Count}");
            foreach (var samurai in samurais)
            {
                Console.WriteLine(samurai.Name);
            }
        }

        private static void Main(string[] args)
        {
            _context.Database.EnsureCreated();
            GetSamurais("Before: ");
            AddSamurai();
            GetSamurais("After: ");
            Console.ReadKey();
        }
    }
}