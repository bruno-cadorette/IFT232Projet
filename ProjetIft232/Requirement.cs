using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetIft232.Buildings;
using ProjetIft232.Technologies;
using System.Runtime.Serialization;

namespace ProjetIft232
{
    [DataContract]
    public class Requirement
    {
        [DataMember]
        public IEnumerable<int> Buildings {get; private set;}
        [DataMember]
        private IEnumerable<int> _Technologies;
        [DataMember]
        public Resources Resources { get; set; }

        public Requirement(IEnumerable<int> buildings, Resources resources)
        {
            Buildings = buildings;
            Resources = resources;
        }


        public Requirement(IEnumerable<int> buildings, IEnumerable<int> technologies, Resources resources)
        {
            Buildings = buildings;
            Resources = resources;
            _Technologies = technologies;
        }

        public static Requirement Zero()
        {
            return new Requirement(new int[0], new int[0], Resources.Zero());
        }


        public bool IsValid(Resources actualResource, IEnumerable<Building> actualBuildings)
        {
            return actualResource >= Resources &&
                Buildings.All(type =>
                actualBuildings.Any(x => 
                    x.ID == type && !x.InConstruction)
                );
        }
        public bool IsValid(Resources actualResource, IEnumerable<Building> actualBuildings, IEnumerable<Technology> actualTechnologies)
        {
            return IsValid(actualResource, actualBuildings) && _Technologies.All(tech => actualTechnologies.Any(x => x.ID == tech && !x.InConstruction));
        }

        public string toString()
        {
            string result="";
            result += "Or : " + Resources.get("Gold") + " Viande : " + Resources.get("Meat") + " Bois : " + Resources.get("Wood") + " Roche : " + Resources.get("Rock") + " Population : " + Resources.get("Population");
            result += " / Batiments : ";
            foreach (var each in Buildings)
            {
                result += " " + BuildingLoader.GetInstance()._entities[each].Name + " ";
            }
            result += " Technologies : ";
            foreach (var each in _Technologies)
            {
                result += " " + TechnologyLoader.GetInstance()._entities[each].Name + " ";
            }
            return result;
        }
    }
}
