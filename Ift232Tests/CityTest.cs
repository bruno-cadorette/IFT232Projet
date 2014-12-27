using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core;
using Core.Army;

namespace Ift232Tests
{
    [TestClass]
    public class CityTest
    {
        public City city1 { get; set; }
        public City city2 { get; set; }

        public Player player { get; set; }


        [TestInitialize]
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
            city1.TransferResources(city2,
                new Resources {Wood = 100, Gold = 1000, Meat = 100, Rock = 100, Population = 100});
            Assert.IsTrue(city1.Ressources < city2.Ressources);
        }

        [TestMethod]
        public void TestEchangeNopeCity()
        {
            city1.TransferResources(city2,
                new Resources {Wood = 1000000, Gold = 1000000, Meat = 1000000, Rock = 1000000, Population = 1000000});
            Assert.IsFalse(city1.Ressources < city2.Ressources);
        }

        [TestMethod]
        public void TestBracketOperator()
        {
            city1.Ressources[ResourcesType.Gold] = 40;
            Assert.AreEqual(city1.Ressources[ResourcesType.Gold], 40);
        }

        [TestMethod]
        public void TestAttack()
        {
            Resources old = new Resources(city1.Ressources);
            Armies opponent = new Armies();

            for (int i = 0; i < 10; i++)
            {
                opponent.Add(ArmyFactory.CreateBarbarian());
            }
            city1.Defend(opponent);
            Assert.IsTrue(old != city1.Ressources);
        }
    }
}