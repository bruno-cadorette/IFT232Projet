using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjetIft232;
using ProjetIft232.Buildings;
using ProjetIft232.Technologies;

namespace Ift232Tests
{
    [TestClass]
    public class TestTechnology
    {
        private City city;
        private Technology tech;

        [TestInitialize]
        public void SetUp()
        {
            city = new City("Toulouse!");
            tech = new Technology(2,"Maconnerie",null,Requirement.Zero(),2,new List<int>(){(int)BuildingType.Farm},new Resources(0,10,100,10,10),new Enhancement(new Resources(20,20,20,20,20),53));
        }
        [TestMethod]
        public void UpgradeBuilding()
        {
            Building farm = BuildingFactory.CreateBuilding((int)BuildingType.Farm, ref city);
            farm.FinishConstruction();
            Building house = BuildingFactory.CreateBuilding((int)BuildingType.House, ref city);
            house.FinishConstruction();

            Technology technology = TechnologyFactory.ResearchTechnology(0, city);
            
            BuildingFactory.UpgrateBuilding(ref house,technology, ref city);
            Resources expected = new Resources(0, 0, 0, 0, 2);
            Assert.AreEqual(expected,house.Resource);
        }

        [TestMethod]
        public void UpgradedNewBuilding()
        {
            city.ResearchedTechnologies.Add(tech);
            for (int i = 0; i < 10; i++)
            {
                city.ResearchedTechnologies.ForEach(n => n.Update());
            }
            Building farm = BuildingFactory.CreateBuilding((int)BuildingType.Farm, ref city);
            for (int i = 0; i < 10; i++)
            {
                farm.Update();
            }
            Assert.IsTrue(farm.Resource[ResourcesType.Rock]>0);
        }

        [TestMethod]
        public void NotUpgradedBuilding()
        {
            city.ResearchedTechnologies.Add(tech);
            Building farm = BuildingFactory.CreateBuilding((int)BuildingType.Farm, ref city);
            for (int i = 0; i < 10; i++)
            {
                city.ResearchedTechnologies.ForEach(n => n.Update());
            }
            Assert.IsFalse(farm.Resource[ResourcesType.Rock] > 0);
        }

        [TestMethod]
        public void TurnUpdate()
        {
            city.ResearchedTechnologies.Add(tech);
            for (int i = 0; i < 10; i++)
            {
                city.ResearchedTechnologies.ForEach(n => n.Update());
            }
            Building farm = BuildingFactory.CreateBuilding((int)BuildingType.Farm, ref city);
            Assert.IsTrue(farm.TurnsLeft == 0);
        }
    }
}
