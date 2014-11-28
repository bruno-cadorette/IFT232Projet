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
            market = (Market)BuildingLoader.GetInstance().GetBuilding((int)BuildingType.Market);
            market.Update();
            market.Update();
            market.Update();
        }
        [TestMethod]
        public void TestAchatVille()
        {
            int bois = city1.Ressources.get("Wood");
            int bois2;
            int viande2;
            int rock = city1.Ressources.get("Rock");
            int rock2;
            int viande = city1.Ressources.get("Meat");
            int gold = city1.Ressources.get("Gold");
            int gold2;
            // Assert.IsFalse(market.Achat(city1,ResourcesType.Wood, 20));

           

          //  market.Achat(city1, 20, TODO, ResourcesType.Wood);
         //   market.Achat(city1, 20, TODO, ResourcesType.Rock);
         //   market.Achat(city1, 20, TODO, ResourcesType.Meat);
      //      bois2 = city1.Ressources.get("Wood");
         //   viande2 = city1.Ressources.get("Meat");
       //     rock2 = city1.Ressources.get("Rock");
        //    gold2 = city1.Ressources.get("Gold");
//
        //    Assert.IsTrue((viande2 - viande )== (8 * 20));
       //     Assert.IsTrue((bois2 - bois) == (15 * 20));
        //    Assert.IsTrue((rock2 - rock) == (12 * 20));
       //     Assert.IsTrue((gold - gold2 == 60));
             }

        public void TestAchatMarket()
        {
            int bois = market.Resource.get("Wood");
            int bois2;
            int viande2;
            int rock = market.Resource.get("Rock");
            int rock2;
            int viande = market.Resource.get("Meat");
            int or2;

            int or = market.Resource.get("Gold");

            /*
            market.Achat(city1, 20, TODO, ResourcesType.Wood);
            market.Achat(city1, 20, TODO, ResourcesType.Rock);
            market.Achat(city1, 20, TODO, ResourcesType.Meat);
            bois2 = market.Resource.get("Wood");
            viande2 = market.Resource.get("Meat");
            rock2 = market.Resource.get("Rock");
            or2 = market.Resource.get("Gold");

            Assert.IsTrue((viande - viande2) == (8 * 20));
            Assert.IsTrue((bois - bois2) == (15 * 20));
            Assert.IsTrue((rock - rock2) == (12 * 20));
            Assert.IsTrue((or2 - or) == ( 3* 20));
             */

        }

        public void TestConversionMarket()
        {
            int boispropose= 145 ; 
            int boisconverti;
            int viandepropose = 9;
            int viandeconverti;
            int rockpropose = 23;
            int rockconverti;
            boisconverti = market.Conversion(boispropose, ResourcesType.Wood);
            viandeconverti = market.Conversion(viandepropose, ResourcesType.Meat);
            rockconverti = market.Conversion(rockpropose, ResourcesType.Rock);


            Assert.IsTrue(boisconverti == 150);
            Assert.IsTrue(viandeconverti == 16);
            Assert.IsTrue(viandeconverti == 24);

            boispropose = 75;
            viandepropose = 80;
            rockpropose = 36;

            boisconverti = market.Conversion(boispropose, ResourcesType.Wood);
            viandeconverti = market.Conversion(viandepropose, ResourcesType.Meat);
            rockconverti = market.Conversion(rockpropose, ResourcesType.Rock);

            Assert.IsTrue(boisconverti == 75);
            Assert.IsTrue(viandeconverti == 80);
            Assert.IsTrue(viandeconverti == 36);
          
        }

        public void TestTradeMarket()
        {
            int boispropose = 100;
            int viandepropose = 40;
            int boisconverti;
            int viandeconvertie;
            int rockconvertie;

            viandeconvertie = market.Trade(boispropose, ResourcesType.Wood, ResourcesType.Meat);

            Assert.Equals(viandeconvertie, 66);

            boisconverti = market.Trade(viandepropose, ResourcesType.Meat,ResourcesType.Wood );
            Assert.Equals(boisconverti, 60);
            rockconvertie = market.Trade(boispropose, ResourcesType.Wood, ResourcesType.Rock);
            Assert.Equals(rockconvertie, 125);



        }
    }
}
