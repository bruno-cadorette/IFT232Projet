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
        }

        protected override int[] UpdateBuilding()
        {
            var ressources = new int[5];
            ressources[(int) Resources.Rock] = 1;
            return ressources;
        }
    }
}
