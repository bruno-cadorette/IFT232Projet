using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Map;
using System.Collections.Generic;
using Core.Military;
using System.Linq;
using Core;

namespace Ift232Tests
{
    [TestClass]
    public class WorldMapTest
    {
        private IEnumerable<KeyValuePair<Position, WorldMapItem>> emptyPoints = Enumerable.Empty<KeyValuePair<Position, WorldMapItem>>();
        [TestMethod]
        public void KeyTest()
        {
            var table = new Dictionary<Position, int>();
            table.Add(new Position(1, 5), 1);
            table.Add(new Position(1, 2), 5);
            Assert.IsTrue(table.ContainsKey(new Position(1, 2)));
            Assert.AreEqual(table[new Position(1, 2)], 5);
            Assert.IsTrue(new Position(4, 6) == new Position(4, 6));
        }
        [TestMethod]
        public void Move()
        {
            var m = new Army()
            {
                Goal = new Position(3, 5),
                Speed = 1
            };
            var position = m.NextPosition(new Position(1, 0), emptyPoints);
            Assert.AreEqual(new Position(1, 1), position);
        }

        [TestMethod]
        public void MoveClose()
        {
            var m = new Army()
            {
                Goal = new Position(3, 5),
                Speed = 150
            };
            var position = m.NextPosition(new Position(1, 0), emptyPoints);
            Assert.AreEqual(new Position(3, 5), position);
        }
        [TestMethod]
        public void MoveBackward()
        {
            var m = new Army()
            {
                Goal = new Position(3, 0),
                Speed = 2
            };
            var position = m.NextPosition(new Position(5, 0), emptyPoints);
            Assert.AreEqual(new Position(3, 0), position);
        }
        [TestMethod]
        public void AtDestination()
        {
            var m = new Army()
            {
                Goal = new Position(3, 0),
                Speed = 2
            };
            var position = m.NextPosition(new Position(3, 0), emptyPoints);
            Assert.AreEqual(new Position(3, 0), position);
        }

        [TestMethod]
        public void MoveOnMyOwn()
        {
            var city = new City("Toulouse")
                {
                    PlayerId = 1
                };
            var worldMap = new WorldMap();
            var army = new Army()
            {
                Speed = 1
            };

            for (int i = 0; i < 10; i++)
            {
                army.Add(ArmyFactory.CreateBarbarian());
            }
            city.Army.Merge(army);
            worldMap.Add(new Position(0, 0), city);
            worldMap.SetMove(new Position(0, 0), new Position(1, 0));
            Assert.AreEqual(city, worldMap[new Position(0, 0)]);
            Assert.AreEqual(army.PlayerId, worldMap[new Position(1, 0)].PlayerId);
            
        }

        [TestMethod]
        public void InteractWithEnemy()
        {
            var army1 = new Army()
            {
                PlayerId = 0
            };
            var army2 = new Army()
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

            Assert.AreEqual(1, map.Count(x=>x.Item!=null));
            Assert.IsNotNull(map[new Position(0, 0)]);
            Assert.AreEqual(1, map[new Position(0, 0)].PlayerId);
        }
    }
}
