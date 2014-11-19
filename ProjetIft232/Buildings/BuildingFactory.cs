using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetIft232.Technologies;

namespace ProjetIft232.Buildings
{
    public class BuildingFactory
    {
        public static Building CreateBuilding(BuildingType type,ref City city)
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
        public static void UpgrateBuilding(ref Building building, Technology technology, ref City city)
        {
           if(building.CanBeUpgraded(city.Ressources,technology))
           {
               building.Upgrade(technology);
               city.RemoveResources(technology.ApplicationCost);
           }
        }

        private static Building GetBuilding(BuildingType type)
        {
            return new Building(BuildingLoader.GetInstance()._buildings[type]);

        }
    }
}
