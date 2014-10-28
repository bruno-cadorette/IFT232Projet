using System;
using System.Collections.Generic;
using System.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjetIft232;

namespace Ift232Tests
{
    [TestClass]
    public class TestResource
    {

        [TestMethod]
        public void TestConstructeur()
        {
            Dictionary<Resources, int> rsc = new Dictionary<Resources, int>();
            rsc.Add(Resources.Gold, 1);
            Resource res = new Resource();
            Resource res2 = new Resource(rsc);

            Assert.AreEqual(res2, new Resource(rsc));


            rsc.Add(Resources.Meat, 2);
            Resource res3 = new Resource(rsc);

            Assert.AreEqual(res3, new Resource(rsc));


            rsc.Add(Resources.Rock, 3);
            Resource res4 = new Resource(rsc);

            Assert.AreEqual(res4, new Resource(rsc));


            rsc.Add(Resources.Wood, 5);
            Resource res5 = new Resource(rsc);

            Assert.AreEqual(res5, new Resource(rsc));


            rsc.Add(Resources.Population, 16);
            Resource res6 = new Resource(rsc);
            Assert.IsTrue(res == new Resource() && res != res2 && res != res3 && res != res4 && res != res5 && res != res6);
            Assert.AreEqual(res6, new Resource(rsc));

        }

        [TestMethod]
        public void TestAdd()
        {
            Dictionary<Resources,int> rsc = new Dictionary<Resources, int>();
            rsc.Add(Resources.Gold, 1);
            Resource res = new Resource(rsc);
            Resource res2 = new Resource(rsc);
            Resource rs3 = res + res2;

            Resource rs4 = res - res2;
            Assert.IsTrue(rs3.Gold == 2);
            Assert.IsTrue(rs4.Gold == 0);
            Assert.IsTrue( rs3.Meat == 0 && rs3.Rock == 0 && rs3.Population == 0 && rs3.Wood == 0);
            Assert.IsTrue(rs4.Meat == 0 && rs4.Rock == 0 && rs4.Population == 0 && rs4.Wood == 0);
        }

        [TestMethod]
        public void TestEqual()
        {
            Dictionary<Resources, int> rsc = new Dictionary<Resources, int>();
            rsc.Add(Resources.Gold, 1);

            Resource rsc2 = new Resource();
            
            Resource res = new Resource(rsc);

            Assert.IsFalse(rsc2.Equals(null));
            Assert.IsFalse(rsc2.Equals(new object()));
            Assert.IsTrue(rsc2.Equals(new Resource()));
            Assert.IsTrue(rsc2 == new Resource());
            Assert.IsFalse(rsc2 == res);
            Assert.IsTrue(rsc2 != res);
            Assert.IsFalse(rsc2 != new Resource());
        }

        [TestMethod]
        public void TestGreater()
        {
            Dictionary<Resources, int> rsc = new Dictionary<Resources, int>();
            rsc.Add(Resources.Gold, 1);

            Resource rsc2 = new Resource();

            Resource res = new Resource(rsc);

            Assert.IsFalse(rsc2 > new Resource());
            Assert.IsFalse(rsc2 < res);
            Assert.IsFalse(res > rsc2);
            Assert.IsFalse(new Resource() > rsc2);
        }

        [TestMethod]
        public void TestGreaterOrEquals()
        {
            Dictionary<Resources, int> rsc = new Dictionary<Resources, int>();
            rsc.Add(Resources.Gold, 1);

            Resource rsc2 = new Resource();

            Resource res = new Resource(rsc);

            Assert.IsTrue(rsc2 >= new Resource());
            Assert.IsFalse(rsc2 >= res);
            Assert.IsFalse(res <= rsc2);
            Assert.IsTrue(new Resource() >= rsc2);
        }

        [TestMethod]
        public void TestUpdate()
        {
            Dictionary<Resources, int> rsc = new Dictionary<Resources, int>();
            rsc.Add(Resources.Gold, 1);

            Resource rsc2 = new Resource();

            Resource res = new Resource(rsc);
            Dictionary<Resources, int> rsc6 = new Dictionary<Resources, int>();
            rsc6.Add(Resources.Gold, 5);
            rsc6.Add(Resources.Meat, 5);
            rsc6.Add(Resources.Wood, 5);
            rsc6.Add(Resources.Rock, 5);
            rsc6.Add(Resources.Population, 1);

            Assert.AreEqual(rsc2, new Resource());
            rsc2.Update(new Resource());
            Assert.AreEqual(rsc2, new Resource(rsc6));
        }
    }
}
