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

        [TestInitialize]
        public void SetUp()
        {
            city = new City("Toulouse!");
        }
        [TestMethod]
        public void UpgradeBuilding()
        {
            Building farm = city.AddBuilding((int)BuildingType.Farm);
            farm.FinishConstruction();
            Building house = city.AddBuilding((int)BuildingType.House);
            house.FinishConstruction();

            Technology technology = TechnologyFactory.CreateTechnology(0, city);
            house.Upgrade(technology);
            Resources expected = new Resources(0, 0, 0, 0, 2);
            Assert.AreEqual(expected,house.Resource);
        }

        [TestMethod]
        public void UpgradedNewBuilding()
        {
            Building farm = city.AddBuilding((int)BuildingType.Farm);
            farm.FinishConstruction();
            city.ResearchedTechnologies.Add(TechnologyFactory.CreateTechnology(0, city));
            for (int i = 0; i < 10; i++)
            {
                city.ResearchedTechnologies.ForEach(n => n.Update());
            }
            Building house = BuildingFactory.CreateBuilding((int)BuildingType.House, city);
            for (int i = 0; i < 10; i++)
            {
                house.Update();
            }
            Assert.IsTrue(house.Resource[ResourcesType.Population] == 2);
        }

        [TestMethod]
        public void NotUpgradedBuilding()
        {
            Building farm = city.AddBuilding((int)BuildingType.Farm);
            farm.FinishConstruction();
            city.ResearchedTechnologies.Add(TechnologyFactory.CreateTechnology(0, city)); 
            Building house = city.AddBuilding((int)BuildingType.Farm);
            for (int i = 0; i < 10; i++)
            {
                city.ResearchedTechnologies.ForEach(n => n.Update());
            }
            Assert.IsFalse(farm.Resource[ResourcesType.Rock] > 0);
        }

        [TestMethod]
        public void TurnUpdate()
        {
            Building farm = city.AddBuilding((int)BuildingType.Farm);
            farm.FinishConstruction();
            city.ResearchedTechnologies.Add(TechnologyFactory.CreateTechnology(0, city));
            for (int i = 0; i < 10; i++)
            {
                city.ResearchedTechnologies.ForEach(n => n.Update());
            }
            Building house = BuildingFactory.CreateBuilding((int)BuildingType.House, city);
            Assert.IsTrue(house.TurnsLeft == 2);
        }
    }
}
