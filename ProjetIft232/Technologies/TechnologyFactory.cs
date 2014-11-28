using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetIft232.Technologies;

namespace ProjetIft232.Technologies
{
    public static class TechnologyFactory
    {
        public static Technology CreateTechnology(int type, City city)
        {
            Technology technology = GetTechnology(type);
            if (technology.CanBeBuild(city.Ressources, city.Buildings, city.ResearchedTechnologies))
            {
                return technology;
            }
            else
            {
                return null;
            }
        }

        private static Technology GetTechnology(int type)
        {
            return TechnologyLoader.GetInstance().GetTechnology(type); ;
        }
    }
}
