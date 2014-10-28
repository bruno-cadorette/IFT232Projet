using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetIft232.Buildings
{
    public class SawMill : Building
    {
        public SawMill()
            : base(3)
        {
            Name = "Scierie";
            Description =
                "Un joyeux camp de bûcheron. \nVive le Canada !\n";
            Dictionary<Resources, int> res = new Dictionary<Resources, int>();
            res.Add(Resources.Wood, 1);
            Ressource = new Resource(res);
        }

        protected override Resource UpdateBuilding()
        {
            return Ressource;
        }
    }
}
