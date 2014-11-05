using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetIft232.Buildings
{
    public class Casern : Building
    {
        public Casern()
            : base(
            BuildingType.Casern, 
            "Caserne", "Un camp d'entraînement pour des soldats d'élite !\nEnfin...Jusqu'à ce qu'on trouve meilleur que ces bras cassés...\n", 
            5,
            Resource.Zero(),
            new Requirement(
                new BuildingType[]{ BuildingType.House, BuildingType.Farm },
                new Resource(15,20,25,15,10)))
        {
        }
    }
}
