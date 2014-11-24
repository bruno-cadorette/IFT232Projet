using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjetIft232;
using ProjetIft232.Buildings;
using ProjetIft232.Army;

namespace Ift232Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ArmyTest
    {

        private City city;
        private Building caserne;

        private void SetUp()
        {
            city = new City("Toulouse");
            Building house = BuildingFactory.CreateBuilding((int)BuildingType.House, ref city);
            house.FinishConstruction();

            Building farm = BuildingFactory.CreateBuilding((int)BuildingType.Farm, ref city);
            farm.FinishConstruction();

            caserne = BuildingFactory.CreateBuilding((int)BuildingType.Casern, ref city);
            caserne.FinishConstruction();
        }

        [TestMethod]
        public void CreateUnit()
        {
            SetUp();
            ArmyUnit unit = ArmyFactory.CreateArmyUnit(0, ref city);
            Assert.IsTrue(unit.Attack == 2);
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
            ArmyUnit unit = ArmyFactory.CreateArmyUnit(0, ref city);
            var resource = new Resources(1000, 50000, 0, 10000, 100000);
            var buildings = new Building[] { caserne, };
            Assert.IsFalse(unit.CanBeBuild(resource, buildings.ToList()));
        }

        [TestMethod]
        public void RequirementTest()
        {
            SetUp();
            ArmyUnit unit = ArmyFactory.CreateArmyUnit(0, ref city);
            var resource = new Resources(1000, 50000, 60000, 10000, 100000);
            var buildings = new Building[] { caserne, };
            Assert.IsTrue(unit.CanBeBuild(resource, buildings));
        }

        [TestMethod]
        public void AttackWinTest()
        {
            SetUp();

            city.AddArmy(0);
            city.AddArmy(0);
            city.army.getUnits().ForEach(unit => unit.Update());
            city.army.getUnits().ForEach(unit => unit.Update());
            city.army.getUnits().ForEach(unit => unit.Update());
            Resources old = new Resources(city.Ressources);
            city.Attack(BarbarianArmyGenerator.CreateArmy(1));
            Assert.IsTrue(old == city.Ressources);
            //Assert.IsTrue(city.army.size() == 2);
        }

        [TestMethod]
        public void AttackLostTest()
        {
            SetUp();
            city.AddArmy(0);
            city.army.getUnits().ForEach(unit => unit.Update());
            city.army.getUnits().ForEach(unit => unit.Update());
            city.army.getUnits().ForEach(unit => unit.Update());
            Resources old = new Resources(city.Ressources);
            Armies opponent = new Armies();

            for (int i = 0; i < 10; i++)
            {
                opponent.addUnit(ArmyFactory.CreateArmyUnit(0,ref city));
            }
            city.Attack(opponent);
            Assert.IsTrue(old > city.Ressources);
            //Assert.IsFalse(city.army.size() == 1);
        }


    }
}
