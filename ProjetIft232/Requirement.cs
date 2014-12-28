using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Core.Buildings;
using Core.Technologies;

namespace Core
{
    [DataContract]
    public class Requirement
    {
        [DataMember] 
        private IEnumerable<int> _Technologies;


        public Requirement(IEnumerable<int> buildings, IEnumerable<int> technologies, Resources resources)
        {
            Buildings = buildings;
            Resources = resources;
            _Technologies = technologies;
        }

        [DataMember]
        public IEnumerable<int> Buildings { get; private set; }

        [DataMember]
        public Resources Resources { get; private set; }

        public static Requirement Zero()
        {
            return new Requirement(new int[0], new int[0], Resources.Zero());
        }

        public bool IsValid(Resources actualResource, IEnumerable<Building> actualBuildings,
            IEnumerable<Technology> actualTechnologies)
        {
            return actualResource >= Resources &&
                   Buildings.All(type => actualBuildings.Any(x => x.ID == type && !x.InConstruction)) &&
                   _Technologies.All(tech => actualTechnologies.Any(x => x.ID == tech && !x.InConstruction));
        }

        public string toString()
        {
            string result = "";
            result += "Or : " + Resources[ResourcesType.Gold] + " Viande : " + Resources[ResourcesType.Meat] +
                      " Bois : " + Resources[ResourcesType.Wood] + " Roche : " + Resources[ResourcesType.Rock] +
                      " Population : " + Resources[ResourcesType.Population];
            result += " Batiments : ";
            foreach (var id in Buildings)
            {
                result += " " + BuildingFactory.GetInstance().GetBuilding(id).Name + " ";
            }
            result += " Technologies : ";
            foreach (var id in _Technologies)
            {
                result += " " + TechnologyFactory.GetInstance().GetTechnology(id).Name + " ";
            }
            return result;
        }
    }
}