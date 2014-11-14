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
            Building house = BuildingFactory.CreateBuilding(BuildingType.House, city);
            house.Update();
            house.Update();
            house.Update();
            house.Update();

            Building farm = BuildingFactory.CreateBuilding(BuildingType.Farm, city);
            farm.Update();
            farm.Update();
            farm.Update();
            farm.Update();

            caserne = BuildingFactory.CreateBuilding(BuildingType.Casern, city);
            caserne.Update();
            caserne.Update();
            caserne.Update();
            caserne.Update();
            caserne.Update();
        }

        [TestMethod]
        public void CreateUnit()
        {
            SetUp();
            ArmyUnit unit = ArmyFactory.CreateArmyUnit(ArmyUnitType.Warrior, city);
            Assert.IsTrue(unit.Attack == 1);
            Assert.IsTrue(unit.InFormation);
            unit.Update();
            unit.Update();
            unit.Update();
            Assert.IsTrue(!unit.InFormation);
            Assert.IsTrue(unit.Type == ArmyUnitType.Warrior);

        }
        [TestMethod]
        public void RequirementTestFail()
        {
            SetUp();
            ArmyUnit unit = ArmyFactory.CreateArmyUnit(ArmyUnitType.Warrior, city);
            var resource = new Resources(1000,50000,0,10000,100000);
            var buildings = new Building[] { caserne, };
            Assert.IsFalse(unit.CanBeBuild(resource, buildings.ToList()));
        }

        [TestMethod]
        public void RequirementTest()
        {
            SetUp();
            ArmyUnit unit = ArmyFactory.CreateArmyUnit(ArmyUnitType.Warrior, city);
            var resource = new Resources(1000,50000,6,10000,100000);
            var buildings = new Building[] { caserne, };
            Assert.IsTrue(unit.CanBeBuild(resource, buildings.ToList()));
        }

        [TestMethod]
        public void AttackWinTest()
        {
            SetUp();

            city.AddArmy(ArmyUnitType.Warrior);
            city.AddArmy(ArmyUnitType.Warrior);
            city.Army.ForEach(unit => unit.Update());
            city.Army.ForEach(unit => unit.Update());
            city.Army.ForEach(unit => unit.Update());
            Resources old = new Resources(city.Ressources);
            city.Attack(BarbarianArmyGenerator.CreateArmy(1));
            Assert.IsTrue(old==city.Ressources);
            Assert.IsTrue(city.Army.Count == 2);
        }

          [TestMethod]
        public void AttackLostTest()
        {
            SetUp();
            city.AddArmy(ArmyUnitType.Warrior);
            city.Army.ForEach(unit => unit.Update());
            city.Army.ForEach(unit => unit.Update());
            city.Army.ForEach(unit => unit.Update());
            Resources old = new Resources(city.Ressources);
            List<ArmyUnit> opponent = new List<ArmyUnit>();
              opponent.Add(new Warrior());
              opponent.Add(new Warrior());
              opponent.Add(new Warrior());
              opponent.Add(new Warrior());
              opponent.Add(new Warrior());
              opponent.Add(new Warrior());
              opponent.Add(new Warrior());
              opponent.Add(new Warrior());
              opponent.Add(new Warrior());
              opponent.Add(new Warrior());
            city.Attack(opponent);
            Assert.IsTrue(old>city.Ressources);
            Assert.IsFalse(city.Army.Count == 1);
        }


    }
}
