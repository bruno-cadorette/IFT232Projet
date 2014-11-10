using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetIft232.Buildings
{
    public class Market : Building
    {
        public Market()
            : base(3)
        {
            Name = "Marché";
            Description =
                "Un bâtiment permettant l'échange de ressources.\nQui sait, peut-être que si vous aidez le marchand il vous donnera un peu d'or...\n";
        }
    }
}
