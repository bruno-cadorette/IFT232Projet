using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace ProjetIft232.Buildings
{
    public class Market : Building
    {
        private Resources resources;
        public int RatioBois { get; private set; }
        public int RatioViande { get; private set; }
        public int RatioPierre{ get; private set; }


        public Market( Resources MarketResources)
            : base(3)
        {
            resources = new Resources(100000, 100000, 100000, 100000, 0);
            this.RatioBois = 15;
            this.RatioPierre = 12;
            this.RatioViande = 8;

            Name = "Marché";
            Description =
                "Un bâtiment permettant l'échange de ressources.\n Qui sait, peut-être que si vous aidez le marchand il vous donnera un peu d'or...\n";
            
        }

        public bool Achat( City city, ResourcesType resourceWanted, int amountGold)
        {
            if (amountGold >= 1) { 
           
            switch(resourceWanted)
            {
                case ResourcesType.Wood:
                    city.AddResources(new Resources(amountGold*RatioBois,0,0,0,0));
                   
                    break;
                case ResourcesType.Meat:
                    city.AddResources(new Resources(0, 0, amountGold*RatioViande, 0, 0));
                    
                    break;

                case ResourcesType.Rock:
                    city.AddResources(new Resources(0, 0,0, amountGold * RatioPierre, 0));
                    

                    break;
            }
            city.RemoveResources(new Resources(0, amountGold, 0, 0, 0));
                return true;

             

            }
            return false;
        }


        public int Conversion (int nb, ResourcesType type)
        {
            int reste;
            int ratio=0;
            switch (type){
                case ResourcesType.Wood:
                    ratio = RatioBois;
                    break;
                case ResourcesType.Rock:
                    ratio = RatioPierre;
                    break;
                case ResourcesType.Meat:
                    ratio = RatioViande;
                    break;
                default:
                    return 0;
            }
            reste = nb % ratio;
            if (reste == 0)
                return nb;
            return ratio * (nb / ratio) + ratio;

        }

        // Création d'un historique pour changer les prix en fonction de l'offre et la demande
        // Avoir un tableau de correspondances de prix entre les ressources et l'argent
        //conservation possible d'un élément de type ressource avec à l'intérieur, l



    }
}
