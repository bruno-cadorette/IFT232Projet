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
        public static  List<ArmyUnit> CreateArmy(int tourIndex)
        {
            List<ArmyUnit> returnList = new List<ArmyUnit>();
            Random random = new Random();

                int nombre = random.Next(1, tourIndex);
                for (int i = 0; i < nombre; i++)
                {
                    returnList.Add( ArmyFactory.CreateBarbarian(0));
                }
            return returnList;
        }
        
    }
}
