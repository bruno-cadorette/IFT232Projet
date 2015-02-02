using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBuilder.MapGenerator
{
    public class SquaredIslandGenerator
    {
        private Random random;
        public SquaredIslandGenerator()
            : this(new Random().Next(int.MaxValue))
        {

        }
        public SquaredIslandGenerator(int seed)
        {
            random = new Random(seed);
        }
        public TileType[,] Generate(int length)
        {
            var map = new TileType[length, length];
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    map[i, j] = (length / 3 <= i && i < 2 * length / 3 && length / 3 <= j && j < 2 * length / 3) ?
                        TileType.Land : TileType.Water;
                }
            }


            return DeleteOrphans(RandomMap(map));
        }
        private TileType[,] DeleteOrphans(TileType[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (AdjacentCells(i, j, map).All(x => x == TileType.Water))
                    {
                        map[i, j] = TileType.Water;
                    }
                }
            }
            return map;
        }
        private TileType[,] RandomMap(TileType[,] map)
        {
            int length = map.GetLength(0) / 3;
            int x = length ;
            int y = length ;
            while (length < map.GetLength(0))
            {
                x = East(x, y, length, ref map);
                y = South(x, y, length, ref map);
                length += 1;
                x = West(x, y, length, ref map);
                y = Nord(x, y, length, ref map);
                length += 1;
            }
            return map;
        }
        private int East(int x, int y, int length, ref TileType[,] map)
        {
            for (int i = 0; i < length; i++)
            {
                if (CanBeLand(x, y - i, map))
                    map[x + i, y] = TileType.Land;
            }
            return x + length;
        }
        private int West(int x, int y, int length, ref TileType[,] map)
        {
            for (int i = 0; i < length && x - i >= 0; i++)
            {
                if (CanBeLand(x, y - i, map))
                    map[x - i, y] = TileType.Land;
            }
            return Math.Max(x - length, 0);
        }
        private int South(int x, int y, int length, ref TileType[,] map)
        {
            for (int i = 0; i < length; i++)
            {
                if (CanBeLand(x, y - i, map))
                    map[x, y + i] = TileType.Land;
            }
            return y + length;
        }
        private int Nord(int x, int y, int length, ref TileType[,] map)
        {
            for (int i = 0; i < length && y - i >= 0; i++)
            {
                if (CanBeLand(x, y - i,map))
                    map[x, y - i] = TileType.Land;
            }
            return Math.Max(y - length, 0);
        }

        private bool CanBeLand(int x, int y, TileType[,] map)
        {
            return random.Next(AdjacentCells(x,y,map).Count(t=>t==TileType.Water))<2;
        }
        private IEnumerable<TileType> AdjacentCells(int x, int y, TileType[,] map)
        {
            for (int i = Math.Max(x - 1, 0); i <= Math.Min(x + 1, map.GetLength(0) - 1); i++)
            {
                for (int j = Math.Max(y - 1, 0); j <= Math.Min(y + 1, map.GetLength(1) - 1); j++)
                {
                    if (x != i && y != j)
                    {
                        yield return map[i, j];
                    }
                }
            }
        }
    }
}
