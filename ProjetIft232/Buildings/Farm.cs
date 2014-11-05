using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetIft232.Buildings
{
    public class Farm : Building
    {
        public Farm()
            : base(BuildingType.Farm, "Ferme", "Une parcelle de terre réservée à l'élevage et la culture afin de récolter de la nourriture.\n", 3,
            new Resource(0,0,1,0,0),
            new Requirement(new BuildingType[0],Resource.Zero()))
        {
        }
    }
}
