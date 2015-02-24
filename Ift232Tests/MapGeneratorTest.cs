using System;
using System.Linq;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MapGenerator;

namespace Ift232Tests
{
    [TestClass]
    public class MapGeneratorTest
    {
        [TestMethod]
        public void NumberOfPoints()
        {
            var generator = new IslandGenerator(100, 100);
            var numbers = generator.GenerateRandomPoints(25);
            Assert.AreEqual(25, numbers.Count());
        }
        [TestMethod]
        public void Circumcircle()
        {
            var triangle = new Triangle()
            {
                A = new Point(5, 7),
                B = new Point(6, 6),
                C = new Point(2, -2)
            };

            Assert.AreEqual(new Point(2, 3), triangle.Circumcircle.Center);
        }
    }
}
