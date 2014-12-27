using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Map
{
    public class WorldMap : IEnumerable<KeyValuePair<Position, WorldMapItem>>
    {
        private Dictionary<Position, WorldMapItem> map = new Dictionary<Position, WorldMapItem>();
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

        public Position AvailableRandomPosition()
        {
            var random = new Random();
            int cell = random.Next(Length * Height - map.Count);
            int i = 0;
            for (; i < cell; i++)
            {
                if (map.ContainsKey(new Position(i % Length, i / Length)))
                {
                    cell++;
                }
            }
            return new Position(i % Length, i / Length);
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

        public void Add(WorldMapItem item)
        {
            var position = AvailableRandomPosition();
            if (map.ContainsKey(position))
            {
                Add(item);
            }
            else
            {
                Add(position, item);
            }
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
            var item = map[from];
            map.Remove(from);
            if (map.ContainsKey(to))
            {
                var newItem = map[to].InteractWith(item);
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
