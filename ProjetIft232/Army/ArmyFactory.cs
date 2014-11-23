using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetIft232.Army
{
    public class ArmyFactory
    {

        public static ArmyUnit CreateArmyUnit(int id, ref City city)
        {
            ArmyUnit unit = ArmyLoader.GetInstance().GetSoldier(id);
            if (unit != null && unit.CanBeBuild(city.Ressources, city.Buildings.ToList()))
            {
                city.RemoveResources(unit.Requirement.Resources);
                return unit;
            }
            return null;
        }
        public static ArmyUnit CreateBarbarian(int id)
        {
            return ArmyLoader.GetInstance().GetSoldier(id);
        }
    }
}
