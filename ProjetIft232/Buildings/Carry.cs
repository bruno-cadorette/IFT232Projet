using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetIft232.Buildings
{
    public class Carry : Building
    {
        public Carry()
            : base(3)
        {
            Name = "Carrière";
            Description =
                "Un chantier placé sur une source de granit infinie...\nQuoi de mieux pour un revenu de roche à chaque tour ?\n";
            Dictionary<Resources,int> res = new Dictionary<Resources, int>();
            res.Add(Resources.Rock, 1);
            Ressource = new Resource(res);
        }

        protected override Resource UpdateBuilding()
        {
            return Ressource;
        }
    }
}
