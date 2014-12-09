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
        public void TestGetMarket()
        {
            game = new Game();
            player = new Player();
            game.Players.Add(player);

            city1 = new City("Toulouse");
            player.Cities.Add(city1);
            city2 = new City("Lyon");
            player.Cities.Add(city2);
            player.NextCity();


            market = (Market) BuildingFactory.GetInstance().GetBuilding((int) BuildingType.Market);
            market2 = (Market) BuildingFactory.GetInstance().GetBuilding((int) BuildingType.Market);

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
        }

        [TestMethod]
        public void LoadAndSave()
        {
            game = new Game();
            player = new Player {playerName = "blabla"};
            player.CreateCity("Sherbrooke");
            player.CurrentCity.AddBuilding(0);
            game.Players.Add(player);
            game.Save("game.txt");

            var newGame = Game.Load("game.txt");

            Assert.AreEqual(game.CurrentPlayer.playerName, newGame.CurrentPlayer.playerName);
            Assert.AreEqual(game.CurrentPlayer.CurrentCity.Name, newGame.CurrentPlayer.CurrentCity.Name);
            Assert.AreEqual(game.CurrentPlayer.CurrentCity.Buildings[0].ID,
                newGame.CurrentPlayer.CurrentCity.Buildings[0].ID);
        }
    }
}