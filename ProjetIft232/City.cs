using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using ProjetIft232.Army;
using ProjetIft232.Buildings;
using ProjetIft232.Technologies;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace ProjetIft232
{
    [DataContract]
    public class City
    {
        public static Resources CostToCreate = new Resources(1000, 1000, 1000, 1000, 500);
        private Resources BaseProduction()
        {
            return new Resources(new Dictionary<ResourcesType, int>() {
            {ResourcesType.Wood, 0},
            {ResourcesType.Gold, 0},
            {ResourcesType.Meat, 0},
            {ResourcesType.Rock, 0},
            {ResourcesType.Population, 0 /*Convert.ToInt32(1 + _tourDepuisCreation * 0.1)*/}
        });
        }
        [DataMember]
        public List<Building> Buildings { get; private set; }

        public IEnumerable<Building> FinishedBuildings
        {
            get { return Buildings.Where(t => !t.InConstruction); }
        }
        [DataMember]
        public List<ArmyUnit> recruitement { get; private set; }

        [DataMember]
        public Armies Army { get; private set; }

        [DataMember]
        public List<Technology> ResearchedTechnologies { get; private set; }

        [DataMember]
        public Resources Ressources { get; private set; }

        [DataMember]
        public string Name { get; private set; }

        [DataMember]
        private int _turnsSinceCreation;

        public City(string name)
        {
            ResearchedTechnologies = new List<Technology>();
            Name = name;
            Ressources = new Resources(10000, 10000, 10000, 10000, 500);
            Buildings = new List<Building>();

            recruitement = new List<ArmyUnit>();
            Army = new Army.Armies();
            _turnsSinceCreation = 0;
        }

        public override string ToString()
        {
            return "Ville de " + Name;
        }

        public void RemoveResources(Resources resource)
        {
            Ressources -= resource;
            Ressources.Abs();
        }

        public void AddResources(Resources resource)
        {
            Ressources += resource;
        }

        public int CountBuilding(String buildingName, bool inConstruction)
        {
            return Buildings.Count(n => n.Name == buildingName && n.InConstruction == inConstruction);
        }

        public void UpgradeEntities(UpgradableEntity entity, Technology technology, int count)
        {
            IEnumerable<UpgradableEntity> entities = Enumerable.Empty<UpgradableEntity>();
            if (entity is Building)
            {
                entities = Buildings;
            }
            else if (entity is ArmyUnit)
            {
                entities = Army.getUnits();
            }

            int n = 0;
            foreach (UpgradableEntity item in entities)
            {
                if (n < count && item.ID == entity.ID && item.CanBeUpgraded(Ressources, technology))
                {
                    item.Upgrade(technology);
                    RemoveResources(technology.ApplicationCost);
                    n++;
                }

            }
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

        public Building AddBuilding(int type)
        {
            var building = BuildingFactory.CreateBuilding(type, this);
            if (building != null)
            {
                RemoveResources(building.Requirement.Resources);
                Buildings.Add(building);
                return building;
            }
            else
            {
                return null;
            }
        }

        public void RemoveBuilding(int nb)
        {
            int nbrCasern = 0;
            if (Buildings.Count <= nb) return;
            if (Buildings[nb] is Casern) // TODO: Fix this. We wont do it. KTHXBYE
            {
                for (int i = 0; i < Buildings.Count; i++)
                {
                    if (Buildings[nb] is Casern)
                    {
                        nbrCasern++;
                    }
                }
                if (nbrCasern == 1)
                {
                    recruitement.Clear();
                }
            }
            Buildings.Remove(Buildings[nb]);
        }

        public bool IsBuilt(int bt)
        {
            foreach (Building building in Buildings)
            {
                if (building.ID == (int)bt)
                    return true;
            }
            return false;
        }

        public void Update()
        {
            _turnsSinceCreation++;
            //rsc n'est pas une reelle ressource, c'est une ressource 'theorique'
            //rsc est en fait un multiplicateur, il nous dira de combien multiplier
            //notre constante de base de récupération des ressources

            //Càd que sans rien, une ville gagne 5 de chaque ressource sauf de population
            //Avec une maison, elle gagnera 6 de Meat et 5 du reste, etc<
            List<ArmyUnit> finished = new List<ArmyUnit>();
            foreach (ArmyUnit unit in recruitement)
            {
                unit.Update();
                if (unit.InConstruction == false)
                {
                    Army.addUnit(unit);
                    finished.Add(unit);
                }
            }
            foreach (ArmyUnit unit in finished)
            {
                recruitement.Remove(unit);
            }

            Resources rsc = new Resources();
            foreach (Building building in Buildings)
            {
                rsc = rsc + building.Update();
            }
            rsc += BaseProduction();
            Ressources.Update(rsc);
        }

        public bool AddArmy(int type)
        {
            var armyUnit = ArmyFactory.CreateArmyUnit(type, this);
            if (armyUnit != null)
            {
                RemoveResources(armyUnit.Requirement.Resources);
                recruitement.Add(armyUnit);
                return true;
            }
            else
            {
                return false;
            }
        }

        public string Attack(Armies BarbarianArmy)
        {
            string Resume = string.Format("La ville est attaqué par des barbares, ils sont {0} ", BarbarianArmy.size());

            int armySize = Army.size();
            int BarbarianArmySize = BarbarianArmy.size();

            if (Army.size() == 0)
            {
                BarbarianArmy.getUnits().ForEach(n => RemoveResources(n.Transport));
                return Resume += string.Format("Nous n'avions aucune defense, nous nous somme fait ecraser");
            }
            if (BarbarianArmy.Fight(Army))
            {
                BarbarianArmy.getUnits().ForEach(n => RemoveResources(n.Transport));
                Resume += string.Format("Nous avons perdu... Les Barbars sont repartis avec ns ressources !");
            }
            else
            {
                Resume += string.Format("Nous avons gagné!        ");
            }
            Resume += string.Format("Dans la bataille nous avons perdu  {0} soldats et eux {1}", armySize - Army.size(), BarbarianArmySize - BarbarianArmy.size());
            return Resume;

        }

        public void TechChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender is ObservableCollection<Technology>)
            {
                foreach (var tech in e.NewItems)
                {
                    ResearchedTechnologies.Add((Technology)tech);
                }
            }
        }

    }
}
