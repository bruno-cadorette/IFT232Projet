using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core;
using Core.Buildings;

namespace Ift232Tests
{
    [TestClass]
    public class MarketTest
    {
        public City city1 { get; set; }
        public Market market { get; set; }


        [TestInitialize]
        public void Init()
        {
            city1 = new City("Toulouse");
            market = (Market) BuildingFactory.GetInstance().GetBuilding((int) BuildingType.Market);
            market.Update();
            market.Update();
            market.Update();
        }

        [TestMethod]
        public void TestAchatVille()
        {
            int bois = city1.Ressources[ResourcesType.Wood];
            int rock = city1.Ressources[ResourcesType.Rock];
            int viande = city1.Ressources[ResourcesType.Meat];
            int gold = city1.Ressources[ResourcesType.Gold];


            market.Achat(city1, 20, ResourcesType.Gold, ResourcesType.Wood);
            market.Achat(city1, 20, ResourcesType.Gold, ResourcesType.Rock);
            market.Achat(city1, 20, ResourcesType.Gold, ResourcesType.Meat);
            var bois2 = city1.Ressources[ResourcesType.Wood];
            var viande2 = city1.Ressources[ResourcesType.Meat];
            var rock2 = city1.Ressources[ResourcesType.Rock];
            var gold2 = city1.Ressources[ResourcesType.Gold];

            Assert.IsTrue((viande2 - viande) == (8*20));
            Assert.IsTrue((bois2 - bois) == (15*20));
            Assert.IsTrue((rock2 - rock) == (12*20));
            Assert.IsTrue((gold - gold2 == 60));
        }

        [TestMethod]
        public void TestConversionMarket()
        {
            int boispropose = 145;
            int boisconverti;
            int viandepropose = 9;
            int viandeconverti;
            int rockpropose = 23;
            int rockconverti;
            boisconverti = market.Conversion(boispropose, ResourcesType.Wood);
            viandeconverti = market.Conversion(viandepropose, ResourcesType.Meat);
            rockconverti = market.Conversion(rockpropose, ResourcesType.Rock);


            Assert.IsTrue(boisconverti == 2175);
            Assert.IsTrue(viandeconverti == 72);
            Assert.IsTrue(rockconverti == 276);
        }

        [TestMethod]
        public void TestTradeMarket()
        {
            int boispropose = 100;
            int viandepropose = 40;
            int boisconverti;
            int viandeconvertie;
            int rockconvertie;

            viandeconvertie = market.Trade(boispropose, ResourcesType.Wood, ResourcesType.Meat);
            boisconverti = market.Trade(viandepropose, ResourcesType.Meat, ResourcesType.Wood);
            rockconvertie = market.Trade(boispropose, ResourcesType.Wood, ResourcesType.Rock);

            Assert.AreEqual(viandeconvertie, 53);
            Assert.AreEqual(boisconverti, 75);
            Assert.AreEqual(rockconvertie, 80);
        }
    }
}