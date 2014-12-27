using Core.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Army
{
    public class BarbarianArmy : Armies
    {
        protected override Position NewGoal(IEnumerable<KeyValuePair<Position, WorldMapItem>> pointsOfInterest)
        {
            var city = pointsOfInterest.FirstOrDefault(c => c.Value is City);
            return city.Key ?? Position.Random();
        }
        public BarbarianArmy()
        {
            Speed = 1;
            PlayerId = -1;
            Goal = Position.Random();
        }
        public static BarbarianArmy CreateArmy(int turnIndex)
        {
            BarbarianArmy res = new BarbarianArmy();
            Random random = new Random();

            int nombre = random.Next(1, turnIndex + 1);
            for (int i = 0; i < nombre; i++)
            {
                res.Add(ArmyFactory.CreateBarbarian());
            }
            return res;
        }
    }
}
