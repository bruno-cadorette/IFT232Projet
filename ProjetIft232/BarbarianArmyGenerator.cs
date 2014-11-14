using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetIft232.Army;

namespace ProjetIft232
{
    public class BarbarianArmyGenerator
    {
        public static  List<ArmyUnit> CreateArmy(int tourIndex)
        {
            List<ArmyUnit> returnList = new List<ArmyUnit>();
            Random random = new Random();

                int nombre = random.Next(1, tourIndex);
                for (int i = 0; i < nombre; i++)
                {
                    returnList.Add( new Warrior());
                }
            return returnList;
        }
        
    }
}
