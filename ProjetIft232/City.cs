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
        public List<Building> Buildings {get;private set;}

        public Resource Ressources { get; private set; }
        public string Name { get; private set; }

        public City(string name)
        {
            Name = name;
            Ressources = new Resource(10000,10000,10000,10000,10000);
            Buildings = new List<Building>();
        }
        public override string ToString()
        {
            return "Ville de "+Name;
        }

        public void RemoveResources(Resource resource)
        {
            Ressources -= resource;
        }

        public bool AddBuilding(BuildingType type)
        {
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
            //Avec une maison, elle gagnera 10 de Meat et 5 du reste, etc<
            Resource rsc = new Resource();
            foreach (Building building in Buildings)
            {
                rsc = rsc + building.Update();
            }
            Ressources.Update(rsc);
        }



    }
}
