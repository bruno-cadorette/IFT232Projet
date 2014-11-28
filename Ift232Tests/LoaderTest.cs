using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjetIft232;
using ProjetIft232.Buildings;

namespace Ift232Tests
{
    [TestClass]
    public class LoaderTest
    {
        [TestMethod]
        public void RequirementTest()
        {
            var casern = BuildingLoader.GetInstance().GetBuilding((int)BuildingType.Casern);
            var builds = new int[] { 0, 1 };
            var requirements = new Requirement(builds, new Resources(500, 1000, 250, 1500, 10));
            Assert.AreEqual(requirements.Resources, casern.Requirement.Resources);
            var a1 = requirements.Buildings.ToArray();
            for (int i = 0; i < builds.Length; i++)
            {
                Assert.AreEqual(builds[i], a1[i]);
            }
            Assert.AreEqual(builds.Length, a1.Length);
            Assert.AreEqual(requirements.Resources, casern.Requirement.Resources);
        }
    }
}
