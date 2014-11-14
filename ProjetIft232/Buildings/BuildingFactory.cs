using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetIft232.Buildings
{
    public class BuildingFactory
    {
        public static Building CreateBuilding(BuildingType type, City city)
        {
            Building building = GetBuilding(type);
            if (building != null && building.CanBeBuild(city.Ressources, city.Buildings))
            {
                city.RemoveResources(building.Requirement.Resources);
                city.Buildings.Add(building);
                return building;
            }
            return null;
        }

        private static Building GetBuilding(BuildingType type)
        {
            return new Building(BuildingLoader.getInstance()._buildings[type]);

        }
    }
}
