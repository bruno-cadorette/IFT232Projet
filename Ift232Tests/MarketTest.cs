using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjetIft232;
using ProjetIft232.Buildings;

namespace Ift232Tests
{
    [TestClass]
    public class MarketTest
    {
        public City city1 { get; set; }
        public Market market { get; set; }



        [TestInitialize()]
        public void Init()
        {
            city1 = new City("Toulouse");
            market = new Market(Resources.Zero());

        }
        [TestMethod]
        public void TestExchangeWood()
        {
            Assert.IsTrue(market.Exchange(city1,ResourcesType.Wood, 20));

        }
    }
}
