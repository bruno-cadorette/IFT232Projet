using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetIft232.Army
{
    public class ArmyFactory
    {
        public static ArmyUnit CreateArmyUnit(ArmyUnitType type, City city)
        {

            switch (type)
            {
                case ArmyUnitType.Warrior:
                    return new Warrior();
            }
            return null;
        }

        public static ArmyUnit Building(ArmyUnit unit, City city)
        {
            if (unit != null && unit.CanBeBuild(city.Ressources, city.Buildings))
            {
                city.RemoveResources(unit.Requirement.Resources);
                return unit;
            }
            return null;
        }
    }
}
