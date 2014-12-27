using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Map
{
    public sealed class Position : IComparable<Position>
    {
        public int X
        {
            get
            {
                return position.Item1;
            }
        }
        public int Y
        {
            get
            {
                return position.Item2;
            }
        }
        private Tuple<int, int> position;
        public Position(int x, int y)
        {
            position = new Tuple<int, int>(x, y);
        }
        public Position MoveTo(int x, int y)
        {
            return new Position(X + x, Y + y);
        }

        public override int GetHashCode()
        {
            return position.GetHashCode();
        }

        public static Position Random()
        {
            var random = new Random();
            return new Position(random.Next(WorldMap.MinBound.X, WorldMap.MaxBound.X), random.Next(WorldMap.MinBound.Y, WorldMap.MaxBound.Y));
        }

        public int CompareTo(Position other)
        {
            if (other == null)
            {
                return -1;
            }
            if (X == other.X && Y == other.Y)
            {
                return 0;
            }
            else if (X < other.X || (X==other.X && Y < other.Y))
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

        public override bool Equals(object obj)
        {
            var p = obj as Position;
            if ((object)p == null)
            {
                return false;
            }
            else
            {
                return CompareTo(p) == 0;
            }
        }

        public static bool operator== (Position a, Position b)
        {
            if ((object)a == null)
            {
                return (object)b == null;
            }

            return a.Equals(b);
        }
        public static bool operator !=(Position a, Position b)
        {
            return !(a==b);
        }
        public override string ToString()
        {
            return "("+X+", "+Y+")";
        }
    }
}
