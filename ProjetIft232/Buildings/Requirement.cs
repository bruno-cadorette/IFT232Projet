using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetIft232.Buildings
{
    public class Requirement
    {
        private IEnumerable<BuildingType> _Buildings;
        public Resource Resources { get; private set; }

        public Requirement(IEnumerable<BuildingType> buildings, Resource resources)
        {
            _Buildings = buildings;
            Resources = resources;
        }

        public static Requirement Zero()
        {
            return new Requirement(new BuildingType[0], Resource.Zero());
        }


        public bool IsValid(Resource actualResource, IEnumerable<Building> actualBuildings)
        {
            return actualResource >= Resources && _Buildings.All(type =>
                actualBuildings.Any(x => 
                    x.Type == type && !x.InConstruction)
                );
        }
    }
}
