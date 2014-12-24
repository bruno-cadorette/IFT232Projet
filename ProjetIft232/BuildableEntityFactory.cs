using Core.Buildings;
using Core.Technologies;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Core
{
    public abstract class BuildableEntityFactory
    {
        private static object _syncRoot = new object();
        protected static BuildableEntityFactory factory;

        protected BuildableEntityFactory()
        {
            _entities = new Dictionary<int, BuildableEntity>();
            document = XDocument.Load("Config.xml");
            LoadBuilding();
        }

        private XDocument document { get; set; }

        public Dictionary<int, BuildableEntity> _entities { get; set; }

        protected BuildableEntity GetEntity(int type)
        {
            return (_entities.ContainsKey(type)) ? DeepCopy(_entities[type]) : null;
        }

        private void LoadBuilding()
        {
            foreach (XElement child in GetChilds(document.Root))
            {
                int id = int.Parse(GetAttribute(child, "id"));
                string name = GetAttribute(child, "Name");
                string desc = GetAttribute(child, "Description");
                int turns = int.Parse(GetAttribute(child, "turns"));
                Requirement requirement = GetRequirement(child.Element(XName.Get("Requirements")));
                BuildableEntity entity = CreateEntity(child, id, name, desc, turns, requirement);
                _entities.Add(id, entity);
            }
        }

        protected abstract BuildableEntity CreateEntity(XElement element, int id, string name, string description,
            int turns, Requirement requirement);

        protected abstract IEnumerable<XElement> GetChilds(XElement element);

        protected string Special(XElement element)
        {
            var special = element.Element(XName.Get("Special"));
            if (special != null)
            {
                return special.Attribute(XName.Get("type")).Value;
            }
            return "";
        }

        private Requirement GetRequirement(XElement e)
        {
            if (e != null)
            {
                return new Requirement(GetBuildings(e),
                    GetTechnologies(e),
                    GetResources(e));
            }
            return Requirement.Zero();
        }


        private IEnumerable<int> GetBuildings(XElement element)
        {
            var buildings = element.Element(XName.Get("Buildings"));
            if (buildings != null)
            {
                return buildings.Elements(XName.Get("Building")).Select(att => int.Parse(att.Value));
            }
            return Enumerable.Empty<int>();
        }

        private IEnumerable<int> GetTechnologies(XElement element)
        {
            var technologies = element.Element(XName.Get("Technologies"));
            if (technologies != null)
            {
                return technologies.Elements(XName.Get("Technology")).Select(att => int.Parse(att.Value));
            }
            return Enumerable.Empty<int>();
        }

        protected Resources GetResources(XElement element)
        {
            int gold = 0, meat = 0, pop = 0, wood = 0, rock = 0;
            var ressources = element.Element(XName.Get("Resources"));
            if (ressources != null)
            {
                var attributes = ressources.Descendants();
                foreach (XElement attribute in attributes)
                {
                    switch (attribute.Name.LocalName)
                    {
                        case "Gold":
                            gold = int.Parse(attribute.Value);
                            break;
                        case "Wood":
                            wood = int.Parse(attribute.Value);
                            break;
                        case "Population":
                            pop = int.Parse(attribute.Value);
                            break;
                        case "Rock":
                            rock = int.Parse(attribute.Value);
                            break;
                        case "Meat":
                            meat = int.Parse(attribute.Value);
                            break;
                    }
                }
            }
            return new Resources { Wood = wood, Gold = gold, Meat = meat, Rock = rock, Population = pop };
        }

        protected string GetAttribute(XElement element, string att)
        {
            var attribute = element.Attribute(XName.Get(att));
            if (attribute != null)
            {
                return attribute.Value;
            }
            return "";
        }

        protected BuildableEntity DeepCopy(BuildableEntity entity)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(BuildableEntity));
                serializer.WriteObject(stream, entity);
                stream.Position = 0;
                return serializer.ReadObject(stream) as BuildableEntity;
            }
        }

        public BuildableEntity CreateEntity(int type, Resources resources, IEnumerable<Building> buildings, IEnumerable<Technology> technologies)
        {
            var entity = GetEntity(type);
            if (entity is UpgradableEntity)
            {
                (entity as UpgradableEntity).Upgrade(technologies);
            }
            return (entity.CanBeBuild(resources, buildings, technologies)) ? entity : null;
        }
    }
}