using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetIft232.Technologies;
using ProjetIft232.Buildings;

namespace ProjetIft232.Technologies
{
    public static class TechnologyFactory
    {

        public static Technology CreateTechnology(int type, Resources resources, IEnumerable<Building> buildings, IEnumerable<Technology> technologies)
        {
            Technology technology = GetTechnology(type);
            if (technology.CanBeBuild(resources, buildings, technologies))
            {
                return technology;
            }
            else
            {
                return null;
            }
        }

        public static Technology CreateTechnology(int type, City city)
        {
            return CreateTechnology(type, city.Ressources, city.Buildings, city.ResearchedTechnologies);
        }

        private static Technology GetTechnology(int type)
        {
            return TechnologyLoader.GetInstance().GetTechnology(type); ;
        }
    }
}
