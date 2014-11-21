using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjetIft232;

namespace Ift232Tests
{
    [TestClass]
    public class AITest
    {
        public PlayerAI player { get; set; }

        [TestMethod]
        public void CreateAI()
        {
            player = new PlayerAI();

            Assert.IsNotNull(player.playerName);
            Assert.AreEqual(player.Cities.Count, 1);
            Assert.IsNotNull(player.CurrentCity);
            Assert.IsNotNull(player.CurrentCity.Name);
        }
    }
}
