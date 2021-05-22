using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System.Diagnostics;

namespace SamuraiApp.Test
{
    [TestClass]
    public class InMemoryTest
    {
        [TestMethod]
        public void CanInsertSamuraiIntoDatabase()
        {
            var builder = new DbContextOptionsBuilder();
            builder.UseInMemoryDatabase("CanInsertSamurai");
            using (var context = new SamuraiContext(builder.Options))
            {
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