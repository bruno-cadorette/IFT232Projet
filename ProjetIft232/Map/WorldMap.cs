using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Map
{
    public class WorldMap
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
        private IEnumerable<Position> VisibleCells(Position position)
        {
            if(map.ContainsKey(position))
            {
                for (int i = 0; i < 2 * map[position].VisionRange+1; i++)
                {
                    int x = position.X - map[position].VisionRange + i;
                    for (int j = 0; j < 2 * map[position].VisionRange+1; j++)
                    {
                        int y = position.Y - map[position].VisionRange + j;
                        yield return new Position(x, y);
                    }
                }
            }
        }
        public void UpdateFogOfWar()
        {
            foreach (KeyValuePair<Position, WorldMapItem> worldMapItem in map)
            {
                var vision = VisibleCells(worldMapItem.Key).Where(p => map.ContainsKey(p) && p != worldMapItem.Key).ToDictionary(p => p, p => map[p]);
                worldMapItem.Value.UpdateVision(vision);
            }
        }
        public IEnumerable<Position> VisibleCellsByPlayer(int playerId)
        {
            return map.Where(x => x.Value.PlayerId == playerId).SelectMany(x => VisibleCells(x.Key));
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

        public IEnumerable<MapCellInfo> GetAllCellsForPlayer(int playerId)
        {
            var visibleTiles = new HashSet<Position>(VisibleCellsByPlayer(playerId));

            for (int i = 0; i < MaxBound.X; i++)
            {
                for (int j = 0; j < MaxBound.Y; j++)
                {
                    var position = new Position(i, j);
                    yield return new MapCellInfo()
                    {
                        Land = landScape[i, j],
                        Item = map.ContainsKey(position) ? map[position] : null,
                        IsVisible = visibleTiles.Contains(position)
                    };
                }
            }
        }
    }
}
