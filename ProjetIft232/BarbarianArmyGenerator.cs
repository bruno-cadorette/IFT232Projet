using System;
using Core.Army;

namespace Core
{
    public class BarbarianArmyGenerator
    {
        static BarbarianArmyGenerator()
        {
            City barbarianCamp = new City("Barbarian Camp");
            barbarianCamp.AddResources(new Resources
            {
                Wood = 10000,
                Gold = 10000,
                Meat = 10000,
                Rock = 10000,
                Population = 1000
            });
        }

        public static Armies CreateArmy(int tourIndex)
        {
            Armies res = new Armies();
            Random random = new Random();

            int nombre = random.Next(1, tourIndex + 1);
            for (int i = 0; i < nombre; i++)
            {
                res.Add(ArmyFactory.CreateBarbarian(0));
            }
            return res;
        }
    }
}