using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core;
using Core.Army;
using Core.Buildings;
using Core.Utility;

namespace Ift232Tests
{
    /// <summary>
    ///     Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ArmyTest
    {
        private Building caserne;
        private City city;

        private void SetUp()
        {
            RandomGen.SetToPredictable(5);
            city = new City("Toulouse");
            Building house = city.AddBuilding((int) BuildingType.House);
            house.FinishConstruction();

            Building farm = city.AddBuilding((int) BuildingType.Farm);
            farm.FinishConstruction();

            caserne = city.AddBuilding((int) BuildingType.Casern);
            caserne.FinishConstruction();
        }

        [TestMethod]
        public void CreateUnit()
        {
            SetUp();
            ArmyUnit unit = ArmyFactory.CreateArmyUnit(0, city);
            Assert.IsTrue(unit.Attributes.Attack == 2);
            Assert.IsTrue(unit.InConstruction);
            unit.Update();
            unit.Update();
            unit.Update();
            Assert.IsTrue(!unit.InConstruction);
            Assert.IsTrue(unit.ID == 0);
        }

        [TestMethod]
        public void RequirementTestFail()
        {
            SetUp();
            ArmyUnit unit = ArmyFactory.CreateArmyUnit(0, city);
            var resource = new Resources {Wood = 1000, Gold = 50000, Rock = 10000, Population = 100000};
            var buildings = new[] {caserne};
            Assert.IsFalse(unit.CanBeBuild(resource, buildings.ToList()));
        }

        [TestMethod]
        public void RequirementTest()
        {
            SetUp();
            ArmyUnit unit = ArmyFactory.CreateArmyUnit(0, city);
            var resource = new Resources {Wood = 1000, Gold = 50000, Meat = 60000, Rock = 10000, Population = 100000};
            var buildings = new[] {caserne};
            Assert.IsTrue(unit.CanBeBuild(resource, buildings));
        }

        [TestMethod]
        public void AttackWinTest()
        {
            SetUp();

            city.AddArmy(0);
            city.AddArmy(0);
            city.Update();
            city.Update();
            city.Update();
            Resources old = new Resources(city.Ressources);
            city.Attack(BarbarianArmyGenerator.CreateArmy(1));
            Assert.IsTrue(old == city.Ressources);
            Assert.IsTrue(city.Army.Count == 19);
        }

        [TestMethod]
        public void AttackLostTest()
        {
            SetUp();
            city.AddArmy(0);
            city.Update();
            city.Update();
            city.Update();
            Resources old = new Resources(city.Ressources);
            Armies opponent = new Armies();

            for (int i = 0; i < 10; i++)
            {
                opponent.Add(ArmyFactory.CreateBarbarian(0));
            }
            city.Attack(opponent);
            Assert.IsTrue(old != city.Ressources);
        }
    }
}