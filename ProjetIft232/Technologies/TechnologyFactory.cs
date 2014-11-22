using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetIft232.Buildings;

namespace ProjetIft232.Technologies
{
    public class TechnologyFactory
    {
        public static Dictionary<string, Technology> Technologies { get; set; }

        static TechnologyFactory()
        {
            Technologies = new Dictionary<string, Technology>();
            Technologies.Add("FermeCool", new Technology(0,"FermeCool", "",Requirement.Zero(),2,new List<BuildingType>(){BuildingType.House},Resources.Zero(),new Enhancement(new Resources(100,0120,212,22,31),2 )));
        }
        public static Technology CreateTechnology()
        {
            return GetTechnology(0);
        }

        public static Technology ReshearcheTech(City city,string name)
        {
            if(Technologies[name].Requirement.IsValid(city.Ressources,city.Buildings,city.ResearchedTechnologies))
                return new Technology(Technologies[name]);
            else
            {
                return null;
            }
        }

        private static Technology GetTechnology(int type)
        {
            return new Technology();

        }
    }
}
