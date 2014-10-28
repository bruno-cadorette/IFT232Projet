using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetIft232.Buildings
{
    public class School : Building
    {
        public School()
            : base(3)
        {
            Name = "École";
            Description =
                "Un bâtiment assez banal...\nEnfin, il y a quelques personnes un peu bizarre qui font des recherches dedans...\n";
        }

        protected override Resource UpdateBuilding()
        {
            return Ressource;
        }
    }
}
