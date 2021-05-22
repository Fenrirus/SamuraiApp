using Microsoft.VisualStudio.TestTools.UnitTesting;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System.Diagnostics;

namespace SamuraiApp.Test
{
    [TestClass]
    public class DatabaseTest
    {
        [TestMethod]
        public void CanInsertSamuraiIntoDatabase()
        {
            using (var context = new SamuraiContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var samurai = new Samurai();
                Debug.WriteLine($"Before save {samurai.Id}");
                context.Samurais.Add(samurai);
                context.SaveChanges();
                Debug.WriteLine($"After save {samurai.Id}");

                Assert.AreNotEqual(0, samurai.Id);
            }
        }
    }
}