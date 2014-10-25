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
            : base(3)
        {
            Name = "Caserne";
            Description =
                "Un camp d'entraînement pour des soldats d'élite !\nEnfin...Jusqu'à ce qu'on trouve meilleur que ces bras cassés...\n";
        }

        protected override int[] UpdateBuilding()
        {
            var ressources = new int[5];
            return ressources;
        }
    }
}
