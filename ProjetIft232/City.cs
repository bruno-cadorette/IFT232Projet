using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjetIft232.Army;
using ProjetIft232.Buildings;
using ProjetIft232.Technologies;
using System.Collections.ObjectModel;

namespace ProjetIft232
{
    public class City
    {

        public static Resources CostToCreate = new Resources(500, 500, 500, 500, 500);
        private Resources BaseProduction()
        {
            return new Resources(new Dictionary<ResourcesType, int>() {
            {ResourcesType.Wood, 5},
            {ResourcesType.Gold, 5},
            {ResourcesType.Meat, 5},
            {ResourcesType.Rock, 5},
            {ResourcesType.Population, Convert.ToInt32(1 + _tourDepuisCreation * 0.1)}
        });
        }
        public ObservableCollection<Building> Buildings { get; private set; }

        public List<ArmyUnit> Army { get; private set; }

        public List<Technology> ResearchedTechnologies { get; private set; }

        public Resources Ressources { get; private set; }
        public string Name { get; private set; }

        private int _tourDepuisCreation;

        public City(string name)
        {
            Name = name;
            Ressources = new Resources(10000, 10000, 10000, 10000, 10000);
            Buildings = new ObservableCollection<Building>();
            Buildings.CollectionChanged += Buildings_CollectionChanged;

            Army = new List<ArmyUnit>();
            _tourDepuisCreation = 0;
        }

        void Buildings_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(Buildings.Any(t=>t==null))
                throw new InvalidOperationException();
        }
        public override string ToString()
        {
            return "Ville de " + Name;
        }

        public void RemoveResources(Resources resource)
        {
            Ressources -= resource;
        }

        public void AddResources(Resources resource)
        {
            Ressources += resource;
        }


        public bool TransferResources(City city, Resources resource)
        {
            if (this.Ressources >= resource)
            {
                this.RemoveResources(resource);
                city.AddResources(resource);
                return true;
            }
            return false;
        }

        public bool AddBuilding(BuildingType type)
        {
            City city = this;
            var building = BuildingFactory.CreateBuilding(type, ref city);
            return building != null;
        }

        public void RemoveBuilding(int nb)
        {
            int nbrCasern = 0;
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
                }
                Buildings.Remove(Buildings[nb]);
            }
        }

        public bool IsBuilt(BuildingType bt)
        {
            if (bt == BuildingType.Null)
                return false;
           foreach (Building building in Buildings)
            {
                if (building.Type == bt)
                    return true;
            }
           return false;
        }

        public void Update()
        {
            _tourDepuisCreation++;
            //rsc n'est pas une reelle ressource, c'est une ressource 'theorique'
            //rsc est en fait un multiplicateur, il nous dira de combien multiplier
            //notre constante de base de récupération des ressources

            //Càd que sans rien, une ville gagne 5 de chaque ressource sauf de population
            //Avec une maison, elle gagnera 6 de Meat et 5 du reste, etc<
            Army.ForEach(n => n.Update());


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

        public string Attack(List<ArmyUnit> BarbarianArmy)
        {
            string Resume = string.Format("La ville est attaqué par des barbares, ils sont {0} \n ", BarbarianArmy.Count);
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
                BarbarianArmy.RemoveRange(0, lost / 2);
                BarbarianArmy.ForEach(n => RemoveResources(n.Transport));
                Resume += string.Format("Nous avons perdu, dans la bataille nous avons perdu  {0} soldats et eux {1}", lost, lost / 2);
            }
            else
            {
                int diff = theirattack - ourdefence;
                int lost = (diff / 2);
                lost = lost > Army.Count ? BarbarianArmy.Count : lost;
                Army.RemoveRange(0, lost / 2);
                BarbarianArmy.RemoveRange(0, lost);
                Resume += string.Format("Nous avons gagné! Dans la bataille nous avons perdu  {0} soldats et eux {1}", lost / 2, lost);
            }
            return Resume;

        }
    }
}
