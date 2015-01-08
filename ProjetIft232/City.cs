using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Serialization;
using Core.Military;
using Core.Buildings;
using Core.Technologies;
using Core.Map;

namespace Core
{
    [DataContract]
    public class City : WorldMapItem, IMovableItemSpawner
    {
        public static Resources CostToCreate = new Resources
        {
            Wood = 10000,
            Gold = 10000,
            Meat = 10000,
            Rock = 10000,
            Population = 500
        };

        [DataMember]
        private int _turnsSinceCreation;

        public City(string name) : this(name,new Army())
        {
            VisionRange = 5;
        }
        public City(string name, Army army)
        {
            ResearchedTechnologies = new List<Technology>();
            Name = name;
            Ressources = new Resources { Wood = 10000, Gold = 10000, Meat = 10000, Rock = 10000, Population = 500 };
            Buildings = new List<Building>();

            recruitement = new List<Soldier>();
            Army = army;
            _turnsSinceCreation = 0;
        }
        private Land land = new Land()
        {
            AttackerBonus = new SoldierAttributes(0, 0, 0),
            DefenderBonus = new SoldierAttributes(0, 500, 0)
        };

        [DataMember]
        public List<Building> Buildings { get; private set; }

        public IEnumerable<Building> FinishedBuildings
        {
            get { return Buildings.Where(t => !t.InConstruction); }
        }

        [DataMember]
        private List<Soldier> recruitement;

        [DataMember]
        public Army Army { get; private set; }

        [DataMember]
        public List<Technology> ResearchedTechnologies { get; private set; }

        [DataMember]
        public Resources Ressources { get; private set; }

        [DataMember]
        public string Name { get; private set; }

        private Resources BaseProduction()
        {
            return new Resources(new Dictionary<ResourcesType, int>
            {
                {ResourcesType.Wood, 0},
                {ResourcesType.Gold, 0},
                {ResourcesType.Meat, 0},
                {ResourcesType.Rock, 0},
                {ResourcesType.Population, 0 /*Convert.ToInt32(1 + _tourDepuisCreation * 0.1)*/}
            });
        }

        public override string ToString()
        {
            return "Ville de " + Name;
        }

        public Resources RemoveResources(Resources resource)
        {
            Ressources -= resource;
            Ressources.Abs();
            return Ressources;
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
            else if (entity is Soldier)
            {
                // entities = Army;
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
            if (Ressources >= resource)
            {
                RemoveResources(resource);
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
            return null;
        }

        public void RemoveBuilding(int nb)
        {
            if (Buildings.Count <= nb) return;
            if (Buildings[nb] is Casern) // TODO: Fix this. We wont do it. KTHXBYE
            {
                // Si toutes les casernes sont demolies, les recrues retournent a la maison !!
                if (Buildings.Count(t => t is Casern) == 1)
                {
                    recruitement.Clear();
                }
            }
            Buildings.Remove(Buildings[nb]);
        }

        public bool IsBuilt(int id)
        {
            return Buildings.Any(x => x.ID == id && !x.InConstruction);
        }

        public void Update()
        {
            _turnsSinceCreation++;
            //rsc n'est pas une reelle ressource, c'est une ressource 'theorique'
            //rsc est en fait un multiplicateur, il nous dira de combien multiplier
            //notre constante de base de récupération des ressources

            //Càd que sans rien, une ville gagne 5 de chaque ressource sauf de population
            //Avec une maison, elle gagnera 6 de Meat et 5 du reste, etc<
            List<Soldier> finished = new List<Soldier>();
            foreach (Soldier unit in recruitement)
            {
                unit.Update();
                if (unit.InConstruction == false)
                {
                    Army.Add(unit);
                    finished.Add(unit);
                }
            }
            foreach (Soldier unit in finished)
            {
                recruitement.Remove(unit);
            }

            Resources rsc = GetBuildingsResources();
            rsc += BaseProduction();
            Ressources.Update(rsc);
        }

        public Resources GetBuildingsResources()
        {
            return Buildings.Aggregate(new Resources(), (acc, x) => acc + x.Update());
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
            return false;
        }

        public bool Defend(Army barbarianArmy)
        {
            string resume = string.Format("La ville est attaqué par des barbares, ils sont {0} ", barbarianArmy.Count());
            land.DefenderBonus = new SoldierAttributes(Ressources.Population,land.DefenderBonus.Defence,0);
            if (Army.Fight(barbarianArmy, land))
            {
                this.Ressources += barbarianArmy.Resources;
                barbarianArmy.Resources = Resources.Zero();
                resume += string.Format("Nous avons gagné!");
                return true;
            }
            else
            {
                barbarianArmy.Resources += RemoveResources(barbarianArmy.AvailableTransport());
                resume += string.Format("Nous avons perdu... Les Barbars sont repartis avec ns ressources !");
                return false;
            }
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

        public override WorldMapItem InteractWith(WorldMapItem item, Land land)
        {
            if (item.PlayerId == PlayerId)
            {
                Army.Merge(item as Army);
                Ressources += (item as Army).Resources;
            }
            else
            {
                if (!Defend(item as Army))
                {
                    PlayerId = item.PlayerId;
                }
            }
            return null;
        }

        public MovableItem Spawn(Position goal)
        {
            if (Army.Size > 0)
            {
                Army army = Army;
                army.Goal = goal;
                Army = new Army();
                return army;
            }
            else
            {
                return null;
            }
        }
    }
}