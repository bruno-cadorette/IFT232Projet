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
            : base(3)
        {
            Name = "Ferme";
            Description =
                "Une parcelle de terre réservée à l'élevage et la culture afin de récolter de la nourriture.\n";
        }

        protected override int[] UpdateBuilding()
        {
            var ressources = new int[5];
            ressources[(int) Resources.Meat] = 1;
            return ressources;
        }
    }
}
