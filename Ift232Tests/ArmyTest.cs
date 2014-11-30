﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjetIft232;
using ProjetIft232.Buildings;
using ProjetIft232.Army;
using ProjetIft232.Utility;

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
            RandomGen.GetInstance().SetRandom(false);
            city = new City("Toulouse");
            Building house = city.AddBuilding((int)BuildingType.House);
            house.FinishConstruction();

            Building farm = city.AddBuilding((int)BuildingType.Farm);
            farm.FinishConstruction();

            caserne = city.AddBuilding((int)BuildingType.Casern);
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
            var resource = new Resources(1000, 50000, 0, 10000, 100000);
            var buildings = new Building[] { caserne, };
            Assert.IsFalse(unit.CanBeBuild(resource, buildings.ToList()));
        }

        [TestMethod]
        public void RequirementTest()
        {
            SetUp();
            ArmyUnit unit = ArmyFactory.CreateArmyUnit(0, city);
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
            city.Update();
            city.Update();
            city.Update();
            Resources old = new Resources(city.Ressources);
            city.Attack(BarbarianArmyGenerator.CreateArmy(1));
            Assert.IsTrue(old == city.Ressources);
            Assert.IsTrue(city.Army.size() == 19);
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
                opponent.addUnit(ArmyFactory.CreateBarbarian(0));
            }
            city.Attack(opponent);
            Assert.IsTrue(old != city.Ressources);
        }


    }
}
