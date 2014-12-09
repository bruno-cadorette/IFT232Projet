using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjetIft232;

namespace Ift232Tests
{
    [TestClass]
    public class AiTest
    {
        public PlayerAI Player { get; set; }

        [TestMethod]
        public void CreateAi()
        {
            Player = new PlayerAI();

            Assert.IsNotNull(Player.playerName);
            Assert.AreEqual(Player.Cities.Count, 1);
            Assert.IsNotNull(Player.CurrentCity);
            Assert.IsNotNull(Player.CurrentCity.Name);
        }
    }
}