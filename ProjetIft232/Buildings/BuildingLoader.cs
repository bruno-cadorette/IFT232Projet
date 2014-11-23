using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProjetIft232.Buildings
{
    public class BuildingLoader : Loader
    {
        private static BuildingLoader instance;

        private static object SyncRoot = new object();



        public Building GetBuilding(int type)
        {
            return (Building)DeepCopy(_entities[type]);
        }

        public IEnumerable<Building> Buildings()
        {
            return _entities.Select(x => (Building)x.Value);
        }

        protected override BuildableEntity CreateEntity(XElement element, int id, string name, string description, int turns, Requirement requirement)
        {
            Resources resources = GetResources(element);
            string specialType = Special(element);
            Building building = new Building(id, name, description, turns, resources, requirement);

            switch (specialType)
            {
                case "Market":
                    return new Market(building, Resources.Zero());
                default:
                    return building;
            }

        }

        protected override IEnumerable<XElement> GetChilds(XElement root)
        {
            return root.Element(XName.Get("Batiments")).Elements(XName.Get("Batiment"));
        }

        public static BuildingLoader GetInstance()
        {
            if (instance == null)
            {
                lock (SyncRoot)
                {
                    if (instance == null)
                    {
                        instance = new BuildingLoader();
                    }
                }
            }
            return instance;
        }


    }
}
