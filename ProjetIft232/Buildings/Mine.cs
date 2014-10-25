using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetIft232.Buildings
{
    public class Mine : Building
    {
        public Mine()
            : base(3)
        {
            Name = "Mine";
            Description =
                "Une infrastructure basée sous la terre exploitant les minéraux afin d'obtenir de l'or.\nOn a toujours besoin d'or...\n";
        }

        protected override int[] UpdateBuilding()
        {
            var ressources = new int[5];
            ressources[(int) Resources.Gold] = 1;
            return ressources;
        }
    }
}
