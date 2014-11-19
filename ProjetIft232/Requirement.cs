using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetIft232.Buildings;
using ProjetIft232.Technologies;

namespace ProjetIft232
{
    public class Requirement
    {
        private IEnumerable<BuildingType> _Buildings;
        private IEnumerable<int> _Technologies;
        public Resources Resources { get; set; }

        public Requirement(IEnumerable<BuildingType> buildings, Resources resources)
        {
            _Buildings = buildings;
            Resources = resources;
        }

        
        public Requirement(IEnumerable<BuildingType> buildings, IEnumerable<int> technologies, Resources resources)
        {
            _Buildings = buildings;
            Resources = resources;
            _Technologies = technologies;
        }

        public Requirement()
        {
            _Buildings = new BuildingType[0];
            Resources = Resources.Zero();
        }

        public static Requirement Zero()
        {
            return new Requirement(new BuildingType[0], new int[0], Resources.Zero());
        }


        public bool IsValid(Resources actualResource, IEnumerable<Building> actualBuildings)
        {
            return actualResource >= Resources && 
                _Buildings.All(type =>
                actualBuildings.Any(x => 
                    x.Type == type && !x.InConstruction)
                );
        }
        public bool IsValid(Resources actualResource, IEnumerable<Building> actualBuildings, IEnumerable<Technology> actualTechnologies)
        {
            return IsValid(actualResource, actualBuildings) && _Technologies.All(tech => actualTechnologies.Any(x => x.ID == tech && !x.InConstruction));
        }
    }
}
