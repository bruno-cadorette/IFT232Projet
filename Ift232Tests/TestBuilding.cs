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

        [TestMethod]
        public void CreateBuilding()
        {
            City city = new City("Toulouse!");
            Assert.AreEqual(BuildingType.Farm,BuildingFactory.CreateBuilding(BuildingType.Farm, city).Type);
        }

        [TestMethod]
        public void CantBeBuildDueResource()
        {
            var casern = new Casern();
            var resource = new Resource(1000,50000,0,10000,100000);
            var buildings = new Building[]{new House(), new Farm()};
            Assert.IsFalse(casern.CanBeBuild(resource, buildings));
        }

        [TestMethod]
        public void CantBeBuildDueBuildings()
        {
            var casern = new Casern();
            var resource = new Resource(1000, 50000, 1000000, 10000, 100000);
            var buildings = new Building[] { new House(), new Farm() };
            Assert.IsFalse(casern.CanBeBuild(resource, buildings));
        }

        [TestMethod]
        public void CanBeBuild()
        {
            House house = new House();
            house.Update();
            house.Update();
            house.Update();
            house.Update();

            Farm farm = new Farm();
            farm.Update();
            farm.Update();
            farm.Update();
            farm.Update();

            var casern = new Casern();
            var resource = new Resource(1000, 50000, 1000000, 10000, 100000);
            var buildings = new Building[] { house, farm };
            Assert.IsTrue(casern.CanBeBuild(resource, buildings));
        }
    }
}
