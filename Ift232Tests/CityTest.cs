using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjetIft232;

namespace Ift232Tests
{
    [TestClass]
    public class CityTest
    {


        public City city1 { get; set; }
        public City city2 { get; set; }

        public Player player { get; set; }


        [TestInitialize()]
        public void Init()
        {
            player = new Player();
            city1 = new City("Toulouse");
            player.Cities.Add(city1);
            city2 = new City("Lyon");
            player.Cities.Add(city2);
            player.NextCity();
        }
        [TestMethod]
        public void TestAddCity()
        {
            player.CreateCity("Paris");
            Assert.IsTrue(player.Cities.Count == 3);
            City citeTest = player.CurrentCity;
            player.NextCity();
            Assert.IsTrue(citeTest != player.CurrentCity);

        }


        [TestMethod]
        public void TestEchangeWorkCity()
        {
            city1.TransferResources(city2, new Resources(100, 1000, 100, 100, 100));
            Assert.IsTrue(city1.Ressources < city2.Ressources);
        }

        [TestMethod]
        public void TestEchangeNopeCity()
        {
            city1.TransferResources(city2, new Resources(1000000, 10000000, 1000000, 1000000, 1000000));
            Assert.IsFalse(city1.Ressources < city2.Ressources);
        }
    }
}
