using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetIft232.Army;
using ProjetIft232.Buildings;

namespace ProjetIft232
{
    public class BarbarianArmyGenerator
    {
        static City barbarianCamp;
        static BarbarianArmyGenerator()
        {
            barbarianCamp = new City("Barbarian Camp");
            barbarianCamp.AddResources(new Resources(10000, 10000, 10000, 10000, 10000));
        }
        public static  Armies CreateArmy(int tourIndex)
        {
            Armies res = new Armies();
            Random random = new Random();

                int nombre = random.Next(1, tourIndex + 1);
                for (int i = 0; i < nombre; i++)
                {
                    res.addUnit( ArmyFactory.CreateBarbarian(0));
                }
            return res;
        }
        
    }
}
