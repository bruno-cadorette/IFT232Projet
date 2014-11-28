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
            City city = new City("Toulouse!");
            Building house = BuildingFactory.CreateBuilding((int)BuildingType.House, city);
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
            City city = new City("Toulouse!");
            Building house = BuildingFactory.CreateBuilding((int)BuildingType.House, city);
            Dictionary<ResourcesType, int> rsc = new Dictionary<ResourcesType, int>();
            rsc.Add(ResourcesType.Population, 1);
            Assert.AreEqual(house.Update(),new Resources());
            Assert.AreEqual(house.Update(), new Resources());
            Assert.AreEqual(house.Update(), new Resources());
            Assert.AreEqual(house.Update(), new Resources(rsc));
        }

        [TestMethod]
        public void CreateBuilding()
        {
            City city = new City("Toulouse!");
            Assert.AreEqual(BuildingType.Farm, (BuildingType)BuildingFactory.CreateBuilding((int)BuildingType.Farm, city).ID);
        }

        [TestMethod]
        public void CantBeBuildDueResource()
        {
            var casern = new Building(BuildingLoader.GetInstance().GetBuilding((int)BuildingType.Casern));
            var resource = new Resources(1000,50000,0,10000,100000);
            var buildings = new Building[] { new Building(BuildingLoader.GetInstance().GetBuilding((int)BuildingType.House)), new Building(BuildingLoader.GetInstance().GetBuilding((int)BuildingType.Farm)) };
            Assert.IsFalse(casern.CanBeBuild(resource, buildings));
        }

        [TestMethod]
        public void CantBeBuildDueBuildings()
        {
            var casern = BuildingLoader.GetInstance().GetBuilding((int)BuildingType.Casern);
            var resource = new Resources(1000, 50000, 1000000, 10000, 100000);
            var buildings = new Building[] { new Building(BuildingLoader.GetInstance().GetBuilding((int)BuildingType.House)), new Building(BuildingLoader.GetInstance().GetBuilding((int)BuildingType.Farm)) };
            Assert.IsFalse(casern.CanBeBuild(resource, buildings));
        }

        [TestMethod]
        public void CanBeBuild()
        {
            City city = new City("Toulouse!");
            Building house = BuildingFactory.CreateBuilding((int)BuildingType.House, city);
            house.Update();
            house.Update();
            house.Update();
            house.Update();

            Building farm = BuildingFactory.CreateBuilding((int)BuildingType.Farm,  city);
            farm.Update();
            farm.Update();
            farm.Update();
            farm.Update();

            var casern = new Building(BuildingLoader.GetInstance().GetBuilding((int)BuildingType.Casern));
            var resource = new Resources(1000, 50000, 1000000, 10000, 100000);
            var buildings = new Building[] { house, farm };
            Assert.IsTrue(casern.CanBeBuild(resource, buildings));
        }
    }
}
