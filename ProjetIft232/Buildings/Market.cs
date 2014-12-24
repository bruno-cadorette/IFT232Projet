using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Core.Buildings
{
    [DataContract]
    public class Market : Building
    {
        /// <summary>
        ///     The resources values
        ///     Note: This could have been a float array, but this is cleaner (and performance doesnt really matter)
        /// </summary>
        private static readonly Dictionary<ResourcesType, float> ResourcesValues = new Dictionary<ResourcesType, float>
        {
            {ResourcesType.Gold, 1},
            {ResourcesType.Meat, 8},
            {ResourcesType.Rock, 12},
            {ResourcesType.Wood, 15},
        };

        [DataMember] private Resources resources;

        public Market()
        {
        }

        public Market(Building building)
            : base(building)
        {
            resources = new Resources {Wood = 10000, Gold = 10000, Meat = 10000, Rock = 10000};
        }

        public bool Achat(City city, int amount, ResourcesType resourceSold, ResourcesType resourceWanted)
        {
            if (city.Ressources[resourceSold] >= amount)
            {
                city.RemoveResources(new Resources(resourceSold, amount));
                city.AddResources(new Resources(resourceWanted, Trade(amount, resourceSold, resourceWanted)));
                return true;
            }
            return false;
        }


        public int Conversion(int nb, ResourcesType type)
        {
            return (int) Math.Floor(nb*ResourcesValues[type]);
        }


        /// <summary>
        ///     Trades the specified input.
        /// </summary>
        /// <param name="qty">The quantity to trade.</param>
        /// <param name="input">The input type.</param>
        /// <param name="output">The output type.</param>
        /// <returns></returns>
        public int Trade(int qty, ResourcesType input, ResourcesType output)
        {
            return (int) Math.Floor(qty/ResourcesValues[input]*ResourcesValues[output]);
        }

        // Création d'un historique pour changer les prix en fonction de l'offre et la demande
        // Avoir un tableau de correspondances de prix entre les ressources et l'argent
        //conservation possible d'un élément de type ressource avec à l'intérieur, l
    }
}