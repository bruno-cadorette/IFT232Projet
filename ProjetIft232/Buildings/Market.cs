using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetIft232.Buildings
{
    public class Market : Building
    {

        
        public Market( Resources MarketResources)
            : base(3)
        {
            Name = "Marché";
            Description =
                "Un bâtiment permettant l'échange de ressources.\n Qui sait, peut-être que si vous aidez le marchand il vous donnera un peu d'or...\n";
            
        }


        // Création d'un historique pour changer les prix en fonction de l'offre et la demande
        // Avoir un tableau de correspondances de prix entre les ressources et l'argent
        //conservation possible d'un élément de type ressource avec à l'intérieur, l



    }
}
