using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetIft232.Buildings
{
    public class House : Building
    {
        public House() 
            : base(3)
        {
            Name = "Maison";
            Description =
                "Un bâtiment principalement composé de bois permettant à votre population actuelle de créer une famille et donc d'augmenter votre population.\n";
            Dictionary<Resources, int> res = new Dictionary<Resources, int>();
            res.Add(Resources.Population, 1);
            Ressource = new Resource(res);
        }

        protected override Resource UpdateBuilding()
        {
            return Ressource;
        }
    }
}
