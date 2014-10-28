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
        private List<Building> _Buildings;

        public Resource Ressources { get; private set; }
        public string Name { get; private set; }

        public City(string name)
        {
            Name = name;
            Dictionary<Resources, int> rsc = new Dictionary<Resources, int>();
            rsc.Add(Resources.Population, 15);
            Ressources = new Resource(rsc);
            _Buildings = new List<Building>();
        }

        [UserCallable("test")]
        public CommandResult GetWorld()
        {
            return new CommandResult("Hello World"+Ressources.ToString());
        }

        public override string ToString()
        {
            return "Ville de "+Name;
        }

        //TODO: Changer quand l'UI va être bien
        public void AddBuilding(int building)
        {
            switch (building)
            {
                case 1 :
                    _Buildings.Add(new House());
                    break;
                case 2:
                    _Buildings.Add(new Farm());
                    break;
                case 3:
                    _Buildings.Add(new Mine());
                    break;
                case 4:
                    _Buildings.Add(new SawMill());
                    break;
                case 5:
                    _Buildings.Add(new Carry());
                    break;
                case 6:
                    _Buildings.Add(new Casern());
                    break;
                case 7:
                    _Buildings.Add(new School());
                    break;
                case 8:
                    _Buildings.Add(new Market());
                    break;
                case 9:
                    _Buildings.Add(new Hospital());
                    break;
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
            foreach (Building building in _Buildings)
            {
                rsc = rsc + building.Update();
            }
            Ressources.Update(rsc);
        }



    }
}
