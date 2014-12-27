using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core;
using Core.Army;
using Core.Buildings;
using Core.Technologies;

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
            Building farm = city.AddBuilding((int) BuildingType.Farm);
            farm.FinishConstruction();
            Building house = city.AddBuilding((int) BuildingType.House);
            house.FinishConstruction();

            Technology technology = TechnologyFactory.CreateTechnology(0, city);
            house.Upgrade(technology);
            Resources expected = new Resources {Population = 2};
            Assert.AreEqual(expected, house.Resource);
        }

        [TestMethod]
        public void UpgradedNewBuilding()
        {
            Building farm = city.AddBuilding((int) BuildingType.Farm);
            farm.FinishConstruction();
            city.ResearchedTechnologies.Add(TechnologyFactory.CreateTechnology(0, city));
            for (int i = 0; i < 10; i++)
            {
                city.ResearchedTechnologies.ForEach(n => n.Update());
            }
            Building house = BuildingFactory.CreateBuilding((int) BuildingType.House, city);
            for (int i = 0; i < 10; i++)
            {
                house.Update();
            }
            Assert.IsTrue(house.Resource[ResourcesType.Population] == 2);
        }

        [TestMethod]
        public void NotUpgradedBuilding()
        {
            Building farm = city.AddBuilding((int) BuildingType.Farm);
            farm.FinishConstruction();
            city.ResearchedTechnologies.Add(TechnologyFactory.CreateTechnology(0, city));
            city.AddBuilding((int) BuildingType.Farm);
            for (int i = 0; i < 10; i++)
            {
                city.ResearchedTechnologies.ForEach(n => n.Update());
            }
            Assert.IsFalse(farm.Resource[ResourcesType.Rock] > 0);
        }

        [TestMethod]
        public void TurnUpdate()
        {
            Building farm = city.AddBuilding((int) BuildingType.Farm);
            farm.FinishConstruction();
            city.ResearchedTechnologies.Add(TechnologyFactory.CreateTechnology(0, city));
            for (int i = 0; i < 10; i++)
            {
                city.ResearchedTechnologies.ForEach(n => n.Update());
            }
            Building house = BuildingFactory.CreateBuilding((int) BuildingType.House, city);
            Assert.IsTrue(house.TurnsLeft == 2);
        }

        [TestMethod]
        public void CanAffectBuilding()
        {
            Building farm = city.AddBuilding((int) BuildingType.Farm);
            farm.FinishConstruction();
            city.ResearchedTechnologies.Add(TechnologyFactory.CreateTechnology(0, city));
            Technology technology = city.ResearchedTechnologies.Find(n => n.ID == 0);
            Building house = city.AddBuilding((int) BuildingType.House);
            house.FinishConstruction();

            Assert.IsTrue(technology.CanAffect(house));
        }

        [TestMethod]
        public void CanAffectSoldier()
        {
            Building house = city.AddBuilding((int) BuildingType.House);
            house.FinishConstruction();
            Building farm = city.AddBuilding((int) BuildingType.Farm);
            farm.FinishConstruction();
            Building caserne = city.AddBuilding((int) BuildingType.Casern);
            caserne.FinishConstruction();
            city.ResearchedTechnologies.Add(TechnologyFactory.CreateTechnology(2, city));
            Technology technology = city.ResearchedTechnologies.Find(n => n.ID == 2);
            Soldier unit = ArmyFactory.CreateArmyUnit(0, city);
            Assert.IsTrue(technology.CanAffect(unit));
        }

        [TestMethod]
        public void CanAffectSoldierUpcast()
        {
            Building house = city.AddBuilding((int) BuildingType.House);
            house.FinishConstruction();
            Building farm = city.AddBuilding((int) BuildingType.Farm);
            farm.FinishConstruction();
            Building caserne = city.AddBuilding((int) BuildingType.Casern);
            caserne.FinishConstruction();
            city.ResearchedTechnologies.Add(TechnologyFactory.CreateTechnology(2, city));
            Technology technology = city.ResearchedTechnologies.Find(n => n.ID == 2);
            Soldier unit = ArmyFactory.CreateArmyUnit(0, city);
            Assert.IsTrue(technology.CanAffect((UpgradableEntity) unit));
        }

        [TestMethod]
        public void TechnologyCopy()
        {
            Building farm = city.AddBuilding((int) BuildingType.Farm);
            farm.FinishConstruction();
            city.ResearchedTechnologies.Add(TechnologyFactory.CreateTechnology(0, city));
            Technology technology = city.ResearchedTechnologies.Find(n => n.ID == 0);
            Technology test = new Technology(technology);
            Assert.IsNotNull(test.AffectedBuildings);
            Assert.IsNotNull(test.AffectedSoldiers);
            Assert.IsNotNull(test.ApplicationCost);
            Assert.IsNotNull(test.Enhancements);
        }

        [TestMethod]
        public void NotCreated()
        {
            Technology technology = TechnologyFactory.CreateTechnology(0, city);
            Assert.IsNull(technology);
        }

        [TestMethod]
        public void NotEmptyLoader()
        {
            IEnumerable<Technology> technologies = TechnologyFactory.GetInstance().Technologies();
            Assert.AreNotEqual(technologies, Enumerable.Empty<Technology>());
        }
    }
}