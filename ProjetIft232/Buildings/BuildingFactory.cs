using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetIft232.Technologies;

namespace ProjetIft232.Buildings
{
    public static class BuildingFactory
    {
        public static Building CreateBuilding(int type, City city)
        {
            Building building = GetBuilding(type);
            if (building != null)
            {
                foreach (var technology in city.ResearchedTechnologies.Where(n => n.AffectedBuildings.Any(m => m == building.ID) && !n.InConstruction))
                {
                    building.Upgrade(technology);
                }
                if (building.CanBeBuild(city.Ressources, city.Buildings))
                {
                    return building;
                }
            }
            return null;
        }

        private static Building GetBuilding(int type)
        {
            return BuildingLoader.GetInstance().GetBuilding(type);
        }
    }
}
