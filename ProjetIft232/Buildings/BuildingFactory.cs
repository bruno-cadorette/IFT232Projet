using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Core.Buildings
{
    public class BuildingFactory : BuildableEntityFactory
    {
        private static BuildingFactory instance;

        private static readonly object SyncRoot = new object();


        public Building GetBuilding(int type)
        {
            return (Building) GetEntity(type);
        }

        public IEnumerable<Building> Buildings()
        {
            return _entities.Select(x => (Building) x.Value);
        }

        protected override BuildableEntity CreateEntity(XElement element, int id, string name, string description,
            int turns, Requirement requirement)
        {
            Resources resources = GetResources(element);
            string specialType = Special(element);
            Building building = new Building(id, name, description, turns, resources, requirement);

            switch (specialType)
            {
                case "Market":
                    return new Market(building);
                case "Casern":
                    return new Casern(building);
                default:
                    return building;
            }
        }

        protected override IEnumerable<XElement> GetChilds(XElement root)
        {
            return root.Element(XName.Get("Batiments")).Elements(XName.Get("Batiment"));
        }

        public static BuildingFactory GetInstance()
        {
            if (instance == null)
            {
                lock (SyncRoot)
                {
                    if (instance == null)
                    {
                        instance = new BuildingFactory();
                    }
                }
            }
            return instance;
        }


        public static Building CreateBuilding(int type, City city)
        {
            Building building = GetInstance().GetBuilding(type);
            if (building != null)
            {
                foreach (
                    var technology in
                        city.ResearchedTechnologies.Where(
                            n => n.AffectedBuildings.Contains(building.ID) && !n.InConstruction))
                {
                    building.Upgrade(technology);
                }
                if (building.CanBeBuild(city.Ressources, city.Buildings))
                {
                    return building;
                }
            }
            return null;
        }
    }
}