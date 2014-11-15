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
        public Resources Resources { get; set; }

        public Requirement(IEnumerable<BuildingType> buildings, Resources resources)
        {
            _Buildings = buildings;
            Resources = resources;
        }

        public Requirement()
        {
            _Buildings = new BuildingType[0];
            Resources = Resources.Zero();
        }

        public static Requirement Zero()
        {
            return new Requirement(new BuildingType[0], Resources.Zero());
        }


        public bool IsValid(Resources actualResource, IEnumerable<Building> actualBuildings)
        {
            return actualResource >= Resources && _Buildings.All(type =>
                actualBuildings.Any(x => 
                    x.Type == type && !x.InConstruction)
                );
        }
    }
}
