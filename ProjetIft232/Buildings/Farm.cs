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
            Dictionary<Resources, int> res = new Dictionary<Resources, int>();
            res.Add(Resources.Meat, 1);
            Ressource = new Resource(res);
        }

        protected override Resource UpdateBuilding()
        {
            return Ressource;
        }
    }
}
