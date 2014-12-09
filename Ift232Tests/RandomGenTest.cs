using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjetIft232.Utility;

namespace Ift232Tests
{
    [TestClass]
    public class RandomGenTest
    {
        [TestMethod]
        public void TestGetInstance()
        {
            Random rand = RandomGen.GetInstance();
            Assert.IsNotNull(rand);
        }

        [TestMethod]
        public void TestNext()
        {
            RandomGen.SetToPredictable(5);
            int value = RandomGen.GetInstance().Next();
            Assert.AreEqual(value, 5);
        }

        [TestMethod]
        public void TestNextMax()
        {
            RandomGen.SetToPredictable(5);
            int value = RandomGen.GetInstance().Next(100);
            Assert.AreEqual(value, 5);
        }

        [TestMethod]
        public void TestNextDouble()
        {
            RandomGen.SetToPredictable(5);
            double value = RandomGen.GetInstance().NextDouble();
            Assert.AreEqual(value, 5);
        }
    }
}