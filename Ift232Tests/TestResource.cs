﻿using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core;

namespace Ift232Tests
{
    [TestClass]
    public class TestResource
    {
        [TestMethod]
        public void TestConstructeur()
        {
            Dictionary<ResourcesType, int> rsc = new Dictionary<ResourcesType, int>();
            rsc.Add(ResourcesType.Gold, 1);
            Resources res = new Resources();
            Resources res2 = new Resources(rsc);

            Assert.AreEqual(res2, new Resources(rsc));


            rsc.Add(ResourcesType.Meat, 2);
            Resources res3 = new Resources(rsc);

            Assert.AreEqual(res3, new Resources(rsc));


            rsc.Add(ResourcesType.Rock, 3);
            Resources res4 = new Resources(rsc);

            Assert.AreEqual(res4, new Resources(rsc));


            rsc.Add(ResourcesType.Wood, 5);
            Resources res5 = new Resources(rsc);

            Assert.AreEqual(res5, new Resources(rsc));


            rsc.Add(ResourcesType.Population, 16);
            Resources res6 = new Resources(rsc);
            Assert.IsTrue(res == new Resources() && res != res2 && res != res3 && res != res4 && res != res5 &&
                          res != res6);
            Assert.AreEqual(res6, new Resources(rsc));
        }

        [TestMethod]
        public void TestAdd()
        {
            Dictionary<ResourcesType, int> rsc = new Dictionary<ResourcesType, int>();
            rsc.Add(ResourcesType.Gold, 1);
            Resources res = new Resources(rsc);
            Resources res2 = new Resources(rsc);
            Resources rs3 = res + res2;

            Resources rs4 = res - res2;
            Assert.IsTrue(rs3[ResourcesType.Gold] == 2);
            Assert.IsTrue(rs4[ResourcesType.Gold] == 0);
            Assert.IsTrue(rs3[ResourcesType.Meat] == 0 && rs3[ResourcesType.Rock] == 0 &&
                          rs3[ResourcesType.Population] == 0 && rs3[ResourcesType.Wood] == 0);
            Assert.IsTrue(rs4[ResourcesType.Meat] == 0 && rs4[ResourcesType.Rock] == 0 &&
                          rs4[ResourcesType.Population] == 0 && rs4[ResourcesType.Wood] == 0);
        }

        [TestMethod]
        public void TestEqual()
        {
            Dictionary<ResourcesType, int> rsc = new Dictionary<ResourcesType, int>();
            rsc.Add(ResourcesType.Gold, 1);

            Resources rsc2 = new Resources();

            Resources res = new Resources(rsc);

            Assert.IsFalse(rsc2.Equals(null));
            Assert.IsFalse(rsc2.Equals(new object()));
            Assert.IsTrue(rsc2.Equals(new Resources()));
            Assert.IsTrue(rsc2 == new Resources());
            Assert.IsFalse(rsc2 == res);
            Assert.IsTrue(rsc2 != res);
            Assert.IsFalse(rsc2 != new Resources());
        }

        [TestMethod]
        public void TestGreater()
        {
            Dictionary<ResourcesType, int> rsc = new Dictionary<ResourcesType, int>();
            rsc.Add(ResourcesType.Gold, 1);

            Resources rsc2 = new Resources();

            Resources res = new Resources(rsc);

            Assert.IsFalse(rsc2 > new Resources());
            Assert.IsFalse(rsc2 > res);
            Assert.IsFalse(res < rsc2);
            Assert.IsFalse(new Resources() > rsc2);
        }

        [TestMethod]
        public void TestGreaterOrEquals()
        {
            Dictionary<ResourcesType, int> rsc = new Dictionary<ResourcesType, int>();
            rsc.Add(ResourcesType.Gold, 1);

            Resources rsc2 = new Resources();

            Resources res = new Resources(rsc);

            Assert.IsTrue(rsc2 >= new Resources());
            Assert.IsFalse(rsc2 >= res);
            Assert.IsFalse(res <= rsc2);
            Assert.IsTrue(new Resources() >= rsc2);
        }

        [TestMethod]
        public void TestUpdate()
        {
            var city = new City("test");
            var rsc = new Dictionary<ResourcesType, int>();
            rsc.Add(ResourcesType.Gold, 1);

            city.RemoveResources(new Resources
            {
                Wood = 10000,
                Gold = 10000,
                Meat = 10000,
                Rock = 10000,
                Population = 1000
            });

            var rsc6 = new Dictionary<ResourcesType, int>();
            rsc6.Add(ResourcesType.Gold, 0);
            rsc6.Add(ResourcesType.Meat, 0);
            rsc6.Add(ResourcesType.Wood, 0);
            rsc6.Add(ResourcesType.Rock, 0);
            rsc6.Add(ResourcesType.Population, 0);

            Assert.AreEqual(city.Ressources, new Resources());
            city.Update();
            Assert.AreEqual(city.Ressources, new Resources(rsc6));
        }
    }
}