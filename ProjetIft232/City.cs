using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjetIft232.Army;
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

        public List<ArmyUnit> Army { get; private set; }

        public Resources Ressources { get; private set; }
        public string Name { get; private set; }

        public City(string name)
        {
            Name = name;
            Ressources = new Resources(10000,10000,10000,10000,10000);
            Buildings = new List<Building>();
            Army = new List<ArmyUnit>();
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
                return true;
            }
            else
            {
                return false;
            }
        }

        public void RemoveBuilding(int nb)
        {
            int nbrCasern=0;
            if (Buildings.Count > nb)
            {
                if (Buildings[nb].Type == BuildingType.Casern)
                {
                    for (int i = 0; i < Buildings.Count; i++)
                    {
                        if (Buildings[nb].Type == BuildingType.Casern)
                        {
                            nbrCasern++;
                        }
                    }
                    if (nbrCasern > 1)
                    {
                        for (int i = Army.Count - 1; i >= 0; i--)
                        {
                            if (Army[i].InFormation == true)
                            {
                                Army.Remove(Army[i]);
                            }
                        }
                    }
                    Buildings.Remove(Buildings[nb]);
                }
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

        public bool AddArmy(ArmyUnitType type)
        {
            var armyUnit = ArmyFactory.CreateArmyUnit(type, this);
            if (armyUnit != null)
            {
                Army.Add(armyUnit);
                return true;
            }
            else
            {
                return false;
            }
        }

        public  string Attack(List<ArmyUnit> BarbarianArmy)
        {
            string Resume = string.Format("La ville est attaqué par des barbares, ils sont {0} \n ",BarbarianArmy.Count) ;
            Random rand = new Random();
            int ourdefence = Army.Where(n => n.InFormation == false).Sum(n => n.Defense);
            int theirattack = BarbarianArmy.Sum(n => n.Attack);
            int theirdefence = BarbarianArmy.Sum(n => n.Defense);
            int ourattack = Army.Where(n => n.InFormation == false).Sum(n => n.Attack);

            if (ourdefence < theirattack)
            {
                int diff = theirattack - ourdefence;
                int lost = (diff / 2);
                lost = lost > Army.Count ? Army.Count : lost;
                Army.RemoveRange(0, lost);
                BarbarianArmy.RemoveRange(0, lost/2);
                BarbarianArmy.ForEach(n => RemoveResources(n.Transport));
                Resume += string.Format("Nous avons perdu, dans la bataille nous avons perdu  {0} soldats et eux {1}", lost, lost/2);
            }
            else
            {
                int diff = theirattack - ourdefence;
                int lost = (diff / 2);
                lost = lost > Army.Count ? BarbarianArmy.Count : lost;
                Army.RemoveRange(0, lost/2);
                BarbarianArmy.RemoveRange(0, lost);
                Resume += string.Format("Nous avons gagné! Dans la bataille nous avons perdu  {0} soldats et eux {1}", lost/2, lost);
            }
            return Resume;

        }
    }
}
