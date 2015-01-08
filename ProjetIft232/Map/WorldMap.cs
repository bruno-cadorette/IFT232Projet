using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Map
{
    public class WorldMap : IEnumerable<MapCellInfo>
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

        public void UpdateFogOfWar()
        {
            foreach (KeyValuePair<Position, WorldMapItem> worldMapItem in map)
            {
                var vision = new Dictionary<Position, WorldMapItem>();
                for (int i = 0; i < 2*worldMapItem.Value.VisionRange; i++)
                {
                    int x = worldMapItem.Key.X - worldMapItem.Value.VisionRange + i;
                    for (int j = 0; j < 2 * worldMapItem.Value.VisionRange; j++)
                    {
                        int y = worldMapItem.Key.Y - worldMapItem.Value.VisionRange + j;
                        Position p = new Position(x,y);
                        if (map.ContainsKey(p) && p != worldMapItem.Key)
                            vision[p] = map[p];

                    }
                }
                worldMapItem.Value.UpdateVision(vision);
            }
        }

        public IEnumerable<WorldMapItem> GetAllItemsFromPlayer(int id)
        {
            return map.Values.Where(x => x.PlayerId == id);
        }
        public void SetMove(Position from, Position to)
        {
            if (map.ContainsKey(from))
            {
                var item = map[from];
                if (item is IMovableItemSpawner)
                {
                    var newItem = (item as IMovableItemSpawner).Spawn(to);
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
        public void ConvertItem(Position position)
        {
            var newItem = (map[position] as IMapItemConverter).Convert();
            if (newItem!=null)
            {
                map[position] = newItem;
            }
        }
        private IEnumerable<Position> ValidPositions()
        {
            for (int i = 0; i < landScape.GetUpperBound(0); i++)
            {
                for (int j = 0; j < landScape.GetUpperBound(1); j++)
                {
                    if (landScape[i, j].CanBeTraveled)
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

        private Position AvailableRandomPosition()
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
            UpdateFogOfWar();
        }

        public void AddToRandomPosition(WorldMapItem item)
        {
            Add(AvailableRandomPosition(), item);
        }
        public void Add(Position position, WorldMapItem item)
        {
            if(IsValid(position))
            {
                map.Add(position, item);
            }
        }
        private bool IsValid(Position position)
        {
            return MinBound.X <= position.X && MinBound.Y <= position.Y
                && MaxBound.X >= position.X && MaxBound.Y >= position.Y;
        }

        private void MoveItem(Position from, Position to)
        {
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
            else if (IsValid(to))
            {
                map.Add(to, item);
            }
        }
        public WorldMapItem this[Position key]
        {
            get
            {
                return map.ContainsKey(key)?map[key]:null;
            }
        }

        private IEnumerable<MapCellInfo> GetAllCells()
        {
            for (int i = 0; i < MaxBound.X; i++)
            {
                for (int j = 0; j < MaxBound.Y; j++)
                {
                    var position = new Position(i,j);
                    yield return new MapCellInfo()
                    {
                        Land = landScape[i, j],
                        Item = map.ContainsKey(position) ? map[position] : null
                    };
                }
            }
        }

        public IEnumerator<MapCellInfo> GetEnumerator()
        {
            return GetAllCells().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
