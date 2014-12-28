using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core;
using Core.Buildings;

namespace Ift232Tests
{
    [TestClass]
    public class LoaderTest
    {
        [TestMethod]
        public void RequirementTest()
        {
            var casern = BuildingFactory.GetInstance().GetBuilding((int) BuildingType.Casern);
            var builds = new[] {0, 1};
            var requirements = new Requirement(builds, Enumerable.Empty<int>(),
                new Resources {Wood = 500, Gold = 1000, Meat = 250, Rock = 1500, Population = 10});
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