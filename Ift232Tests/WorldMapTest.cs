using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Map;
using System.Collections.Generic;
using Core.Army;
using System.Linq;

namespace Ift232Tests
{
    [TestClass]
    public class WorldMapTest
    {
        private IEnumerable<KeyValuePair<Position, WorldMapItem>> emptyPoints = Enumerable.Empty<KeyValuePair<Position, WorldMapItem>>();
        [TestMethod]
        public void KeyTest()
        {
            var table = new Dictionary<Position,int>();
            table.Add(new Position(1, 5), 1);
            table.Add(new Position(1, 2), 5);
            Assert.IsTrue(table.ContainsKey(new Position(1, 2)));
            Assert.AreEqual(table[new Position(1, 2)], 5);
            Assert.IsTrue(new Position(4, 6) == new Position(4, 6));
        }
        [TestMethod]
        public void Move()
        {
            var m = new Armies()
            {
                Goal = new Position(3, 5),
                Speed = 1
            };
            var position = m.NextPosition(new Position(1, 0), emptyPoints);
            Assert.AreEqual(new Position(1,1), position);
        }

        [TestMethod]
        public void MoveClose()
        {
            var m = new Armies()
            {
                Goal = new Position(3, 5),
                Speed = 150
            };
            var position = m.NextPosition(new Position(1, 0),emptyPoints);
            Assert.AreEqual(new Position(3, 5), position);
        }
        [TestMethod]
        public void MoveBackward()
        {
            var m = new Armies()
            {
                Goal = new Position(3, 0),
                Speed = 2
            };
            var position = m.NextPosition(new Position(5, 0),emptyPoints);
            Assert.AreEqual(new Position(3, 0),position);
        }
        [TestMethod]
        public void AtDestination()
        {
            var m = new Armies()
            {
                Goal = new Position(3, 0),
                Speed = 2
            };
            var position = m.NextPosition(new Position(3, 0), emptyPoints);
            Assert.AreEqual(new Position(3, 0), position);
        }

        [TestMethod]
        public void InteractWithEnemy()
        {
            var army1 = new Armies()
            {
                PlayerId = 0
            };
            var army2 = new Armies()
            {
                Goal = new Position(0, 0),
                Speed = 10,
                PlayerId = 1
            };
            army1.Add(ArmyFactory.CreateBarbarian());
            for (int i = 0; i < 10; i++)
            {
                army2.Add(ArmyFactory.CreateBarbarian());
            }
            var map = new WorldMap();
            map.Add(new Position(0, 0), army1);
            map.Add(new Position(2, 1), army2);
            map.Update();

            Assert.AreEqual(1, map.Count());
            Assert.AreEqual(new Position(0, 0), map.FirstOrDefault().Key);
            Assert.AreEqual(1, map.FirstOrDefault().Value.PlayerId);
        }
    }
}
