using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Core.Map
{
    [DataContract]
    public abstract class MovableItem : WorldMapItem
    {
        public Position Goal { get; set; }
        public int Speed { get; set; }
        public Position NextPosition(Position current, IEnumerable<KeyValuePair<Position, WorldMapItem>> pointsOfInterest)
        {
            Goal = NewGoal(pointsOfInterest) ?? current;
            int x = Goal.X - current.X;
            int y = Goal.Y - current.Y;
            int distance = Math.Abs(x)+Math.Abs(y);
            if (distance == 0)
            {
                return current;
            }
            else
            {
                int moves = Math.Min(Speed, distance);
                int distanceX = (int)(moves * (float)x / (float)distance);
                return current.MoveTo(distanceX, (moves - Math.Abs(distanceX))* Math.Sign(y));
            }
        }
        protected virtual Position NewGoal(IEnumerable<KeyValuePair<Position, WorldMapItem>> pointsOfInterest)
        {
            return Goal;
        }
    }
}
