using System;
using System.Linq;
using System.Security.AccessControl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjetIft232;
using ProjetIft232.Buildings;

namespace Ift232Tests
{
    [TestClass]
    public class TestBuilding
    {
        [TestMethod]
        public void Build()
        {
            var house = new House();
            Assert.IsTrue(house.InConstruction);
            Assert.IsTrue(house.TurnsLeft==3);
            house.Update();
            house.Update();
            house.Update();
            Assert.IsTrue(house.TurnsLeft==0);
            Assert.IsFalse(house.InConstruction);
        }
        [TestMethod]
        public void UpdateHouse()
        {
            var house = new House();
            var answer = new int[5];
            answer[(int)Resources.Population] = 1;
            house.Update();
            house.Update();
            house.Update();
            var a = house.Update();
            for (int i = 0; i < a.Length; i++)
            {
                Assert.AreEqual(a[i],answer[i]);
            }

        }
    }
}
