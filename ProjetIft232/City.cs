using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjetIft232.Buildings;

namespace ProjetIft232
{
    public class City
    {
        private static Resources BaseProduction() {
            return new Resources(new Dictionary<ResourcesType, int>() {
            {ResourcesType.Wood, 5},
            {ResourcesType.Gold, 5},
            {ResourcesType.Meat, 5},
            {ResourcesType.Rock, 5},
            {ResourcesType.Population, Convert.ToInt32(1 + Game.TourIndex * 0.1)}
        });
        }
        public List<Building> Buildings {get;private set;}

        public Resources Ressources { get; private set; }
        public string Name { get; private set; }

        public City(string name)
        {
            Name = name;
            Ressources = new Resources(10000,10000,10000,10000,10000);
            Buildings = new List<Building>();
        }
        public override string ToString()
        {
            return "Ville de "+Name;
        }

        public void RemoveResources(Resources resource)
        {
            Ressources -= resource;
        }

        public bool AddBuilding(BuildingType type)
        {
            var buildXML = new Building(BuildingLoader.getInstance()._buildings[type]);
            var building = BuildingFactory.CreateBuilding(type, this);
            if (building != null)
            {
                Buildings.Add(building);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Update()
        {
            //rsc n'est pas une reelle ressource, c'est une ressource 'theorique'
            //rsc est en fait un multiplicateur, il nous dira de combien multiplier
            //notre constante de base de récupération des ressources

            //Càd que sans rien, une ville gagne 5 de chaque ressource sauf de population
            //Avec une maison, elle gagnera 6 de Meat et 5 du reste, etc<
            Resources rsc = new Resources();
            foreach (Building building in Buildings)
            {
                rsc = rsc + building.Update();
            }
            rsc += BaseProduction();
            Ressources.Update(rsc);
        }

    }
}
