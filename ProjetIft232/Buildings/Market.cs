using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace ProjetIft232.Buildings
{
    [DataContract]
    public class Market : Building
    {
        [DataMember]
        private Resources resources;
        [DataMember]
        public int RatioBois { get; private set; }
        [DataMember]
        public int RatioViande { get; private set; }
        [DataMember]
        public int RatioPierre{ get; private set; }

        public Market()
        {

        }

        public Market(Building building,  Resources marketResources)
            : base(building)
        {
            resources = new Resources(100000, 100000, 100000, 100000, 0);
            this.RatioBois = 15;
            this.RatioPierre = 12;
            this.RatioViande = 8;
            
        }

        public bool Achat(City city, int amount, ResourcesType resourceSold, ResourcesType resourceWanted)
        {
            if (city.Ressources[resourceSold] >= amount)
            {
                city.RemoveResources(new Resources(resourceSold, amount));
                city.AddResources(new Resources(resourceWanted, Trade(amount,resourceSold,resourceWanted)));
                return true;
            }
            return false;
        }


        public int Conversion (int nb, ResourcesType type)
        {
           
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
             return nb*ratio;
           
        }


        /// <summary>
        /// The resources values
        /// Note: This could have been a float array, but this is cleaner (and performance doesnt really matter)
        /// </summary>
        static private readonly Dictionary<ResourcesType, float> ResourcesValues = new Dictionary<ResourcesType, float>()
        {
            { ResourcesType.Gold, 1},
            { ResourcesType.Meat, 8},
            { ResourcesType.Rock, 15},
            { ResourcesType.Wood, 12},
        };

        /// <summary>
        /// Trades the specified input.
        /// </summary>
        /// <param name="qty">The quantity to trade.</param>
        /// <param name="input">The input type.</param>
        /// <param name="output">The output type.</param>
        /// <returns></returns>
        public int Trade(int qty, ResourcesType input, ResourcesType output)
        {
            return (int)Math.Floor(qty / ResourcesValues[input] * ResourcesValues[output]);
        }

        // Création d'un historique pour changer les prix en fonction de l'offre et la demande
        // Avoir un tableau de correspondances de prix entre les ressources et l'argent
        //conservation possible d'un élément de type ressource avec à l'intérieur, l



    }
}
