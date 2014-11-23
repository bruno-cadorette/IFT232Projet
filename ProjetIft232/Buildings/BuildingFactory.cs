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
        public static Building CreateBuilding(int type,ref City city)
        {
            Building building = GetBuilding(type);
            var techs = city.ResearchedTechnologies.Where(n => n.AffectedBuilding.Any(m => m == building.ID) && n.InConstruction == false);
            foreach (var technology in techs)
            {
                building.Upgrade(technology); 
                building.ReduceTurnLeft(technology.Enhancements.ConstructionTime);
            }
            if (building != null && building.CanBeBuild(city.Ressources, city.Buildings))
            {
                city.RemoveResources(building.Requirement.Resources);
                city.Buildings.Add(building);
                return building;
            }
            return null;
        }

        public static bool UpgrateBuilding(ref Building building, Technology technology, ref City city)
        {
           if(building.CanBeUpgraded(city.Ressources,technology))
           {
               building.Upgrade(technology);
               city.RemoveResources(technology.ApplicationCost);
               return true;
           }
            return false;
        }

        private static Building GetBuilding(int type)
        {
            return BuildingLoader.GetInstance().GetBuilding(type);

        }
    }
}
