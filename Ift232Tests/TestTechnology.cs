using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjetIft232;
using ProjetIft232.Buildings;
using ProjetIft232.Technologies;

namespace Ift232Tests
{
    [TestClass]
    public class TestTechnology
    {
        [TestMethod]
        public void UpgradeBuilding()
        {
            City city = new City("Toulouse!");
            Technology technology = TechnologyFactory.CreateTechnology();
            Building house = BuildingFactory.CreateBuilding(BuildingType.House, ref city);
            for (int i = 0; i < 10; i++)
			{
                house.Update();
			}
            
            BuildingFactory.UpgrateBuilding(ref house,technology, ref city);
            Resources expected = new Resources(100,100,100,100,101);
            Assert.AreEqual(expected,house.Resource);
        }
    }
}
