using System;
using System.Collections.Generic;
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
            Building house=new House();
            Dictionary<Resources, int> rsc = new Dictionary<Resources, int>();
            rsc.Add(Resources.Population, 1);
            Assert.AreEqual(house.Update(),new Resource());
            Assert.AreEqual(house.Update(), new Resource());
            Assert.AreEqual(house.Update(), new Resource());
            Assert.AreEqual(house.Update(), new Resource(rsc));

        }
    }
}
