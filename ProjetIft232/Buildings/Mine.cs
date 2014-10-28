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
            Dictionary<Resources, int> res = new Dictionary<Resources, int>();
            res.Add(Resources.Gold, 1);
            Ressource = new Resource(res);
        }

        protected override Resource UpdateBuilding()
        {
            return Ressource;
        }
    }
}
