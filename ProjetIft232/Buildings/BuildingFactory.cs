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
                return building;
            }
            return null;
        }

        private static Building GetBuilding(BuildingType type)
        {
            switch (type)
            {
                case BuildingType.House:
                    return new House();
                case BuildingType.Farm:
                    return new Farm();
                case BuildingType.Mine:
                    return new Mine();
                case BuildingType.SawMill:
                    return new SawMill();
                case BuildingType.Carry:
                    return new Carry();
                case BuildingType.Casern:
                    return new Casern();
                case BuildingType.School:
                    return new School();
                case BuildingType.Market:
                    return new Market();
                case BuildingType.Hospital:
                    return new Hospital();
                default:
                    return null;
            }
        }
    }
}
