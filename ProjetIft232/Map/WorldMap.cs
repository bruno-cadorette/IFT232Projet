using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Map
{
    public class WorldMap : IEnumerable<KeyValuePair<Position, WorldMapItem>>
    {
        private Dictionary<Position, WorldMapItem> map = new Dictionary<Position, WorldMapItem>();
        private Land[,] landScape = new Land[MaxBound.X, MaxBound.Y];
        public WorldMap()
        {
            for (int i = 0; i < landScape.GetUpperBound(0); i++)
            {
                for (int j = 0; j < landScape.GetUpperBound(1); j++)
                {
                    landScape[i, j] = new Land();
                }
            }
        }
        public static Position MinBound
        {
            get
            {
                return new Position(0, 0);
            }
        }
        public static Position MaxBound
        {
            get
            {
                return new Position(15, 15);
            }
        }

        public int Length
        {
            get
            {
                return MaxBound.X - MinBound.X;
            }
        }
        public int Height
        {
            get
            {
                return MaxBound.Y - MinBound.Y;
            }
        }
        public void SetMove(Position from, Position to)
        {
            if (map.ContainsKey(from))
            {
                var item = map[from];
                if (item is MovableItemSpawner)
                {
                    var newItem = (item as MovableItemSpawner).Spawn(to);
                    if (newItem != null)
                    {
                        MakeMovement(newItem, newItem.NextPosition(from, map));
                    }
                }
                else
                {
                    var movable = item as MovableItem;
                    movable.Goal = to;
                }
            }
        }
        private IEnumerable<Position> ValidPositions()
        {
            for (int i = 0; i < landScape.GetUpperBound(0); i++)
            {
                for (int j = 0; j < landScape.GetUpperBound(1); j++)
                {
                    if (landScape[i, j].CanTravel)
                    {
                        var position = new Position(i, j);
                        if (!map.ContainsKey(position))
                        {
                            yield return position;
                        }
                    }
                }
            }
        }

        public Position AvailableRandomPosition()
        {
            var random = new Random();
            var positions = ValidPositions().ToArray();
            return positions[random.Next(positions.Length)];
        }
        public void Update()
        {
            foreach (var toBeDeleted in map.Where(x => x.Value.CanBeDeleted))
            {
                map.Remove(toBeDeleted.Key);
            }
            foreach (var item in map.ToList().Where(x => x.Value is MovableItem))
            {
                MoveItem(item.Key, (item.Value as MovableItem).NextPosition(item.Key, map));
            }
        }

        public void AddToRandomPosition(WorldMapItem item)
        {
            Add(AvailableRandomPosition(), item);
        }
        public void Add(Position position, WorldMapItem item)
        {
            map.Add(position, item);
        }
        private bool IsValid(Position position)
        {
            return MinBound.X <= position.X && MinBound.Y <= position.Y
                && MaxBound.X >= position.X && MaxBound.Y >= position.Y;
        }

        private void MoveItem(Position from, Position to)
        {
            if (!IsValid(to))
            {
                throw new IndexOutOfRangeException();
            }
            var item = map[from] as MovableItem;
            map.Remove(from);

            MakeMovement(item, to);
        }
        private void MakeMovement(MovableItem item, Position to)
        {
            if (map.ContainsKey(to))
            {
                var newItem = map[to].InteractWith(item, landScape[to.X, to.Y]);
                if (newItem != null)
                {
                    map[to] = newItem;
                }
            }
            else
            {
                map.Add(to, item);
            }
        }
        public WorldMapItem this[Position key]
        {
            get
            {
                return map[key];
            }
        }

        public IEnumerator<KeyValuePair<Position, WorldMapItem>> GetEnumerator()
        {
            return map.OrderBy(x => x.Key).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
