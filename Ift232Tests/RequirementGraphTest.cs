using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Core;
using Core.Buildings;
using System.Collections.Generic;

namespace Ift232Tests
{
    [TestClass]
    public class RequirementGraphTest
    {
        [TestMethod]
        public void TransformToGraph()
        {
            var a = new RequirementGraph(BuildingFactory.GetInstance().Buildings());
            Assert.IsTrue(!a.IsCircular());
        }
        [TestMethod]
        public void SimplifyGraph()
        {
            var g = new KeyValuePair<int, int>[]
            {
                new KeyValuePair<int,int>(1,2),
                new KeyValuePair<int,int>(2,3),
                new KeyValuePair<int,int>(3,4),
                new KeyValuePair<int,int>(2,4),

            }.ToLookup(x => x.Key, x => x.Value);
            var graph = new RequirementGraph(g);
            var simplifiedGraph = graph.Simplify();
            Assert.IsTrue(simplifiedGraph.Any());
            Assert.IsFalse(simplifiedGraph.FirstOrDefault(x=>x.Key==2).Any(x=>x==4));
        }
    }

    public static class AssertManager
    {
        public static void FailWith<T>(Action a) where T : Exception
        {
            try
            {
                a();
                Assert.Fail();
            }
            catch (T)
            {
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
