using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjetIft232;


using ProjetIft232.Buildings;

namespace Ift232Tests
{
    [TestClass]
    public class TestGame 
    {
        public Game game { get; set; }
        public City city1 { get; set; }
        public City city2 { get; set; }
        public Market market { get; set; }
        public Market market2 { get; set; }
        public Player player { get; set; }

        public Building instancem1 { get; set; }
        public Building instancem2 { get; set; }

        [TestMethod]

          

        public void TestgetMarket()
        {
            game = new Game();
            player = new Player();
            game.Players.Add(player);
            
            city1 = new City("Toulouse");
            player.Cities.Add(city1);
            city2 = new City("Lyon");
            player.Cities.Add(city2);
            player.NextCity();

            
            market = new Market(Resources.Zero());
            market2 = new Market(Resources.Zero());

            player.CurrentCity.Buildings.Add(market);
            market.Update();
            market.Update();
            market.Update();
            player.NextCity();
            player.CurrentCity.Buildings.Add(market2);

           instancem1 = game.getMarket();

           player.NextCity();

           instancem1 = game.getMarket();

           Assert.IsNotNull(instancem1);
           Assert.IsTrue(instancem2 == null);



            // _Game.getMarket() != null



        }
    }
}
