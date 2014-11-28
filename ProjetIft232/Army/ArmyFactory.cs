using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetIft232.Army
{
    public static class ArmyFactory
    {
        public static ArmyUnit CreateArmyUnit(int id, City city)
        {
            ArmyUnit unit = ArmyLoader.GetInstance().GetSoldier(id);
            if (unit != null)
            {
                foreach (var tech in city.ResearchedTechnologies)
                {
                    unit.Upgrade(tech);
                }
                if (unit.CanBeBuild(city.Ressources, city.Buildings, city.ResearchedTechnologies))
                {
                    return unit;
                }
            }
            return null;
        }
        public static ArmyUnit CreateBarbarian(int id)
        {
            return ArmyLoader.GetInstance().GetSoldier(id);
        }
    }
}
