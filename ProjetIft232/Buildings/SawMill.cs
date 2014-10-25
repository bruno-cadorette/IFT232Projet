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
        }

        protected override int[] UpdateBuilding()
        {
            var ressources = new int[5];
            ressources[(int) Resources.Wood] = 1;
            return ressources;
        }
    }
}
