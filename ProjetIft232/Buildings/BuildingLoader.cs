using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProjetIft232.Buildings
{
    public class BuildingLoader
    {
        XDocument doc { get; set; }
        private static BuildingLoader instance;

        public Dictionary<BuildingType, Building> _buildings { get; set; } 
        private static object SyncRoot = new object();

        private BuildingLoader()
        {
            _buildings = new Dictionary<BuildingType, Building>();
            doc = XDocument.Load("Buildings.xml");
            LoadBuilding();
        }

        public Building GetBuilding(BuildingType type)
        {
            return _buildings[type];
        }

        private void LoadBuilding()
        {
            XElement root = doc.Root;
            var childs = root.Descendants().Where(n=> n.Name == "Batiment");
            foreach (XElement child in childs)
            {
                string name = GetAttribute(child, "Name");
                string desc = GetAttribute(child, "Description");
                int turn = int.Parse(GetAttribute(child, "turn"));
                BuildingType id = (BuildingType)int.Parse(GetAttribute(child, "id"));
                Resources att = GetRessource(child,"Attributes");
                Resources costRes = GetRessource(child, "Res");
                IEnumerable<BuildingType> buildingCost = GetBuilding(child);
                bool isSpecial = IsSpecial(child);
                Building build = new Building(id, name, desc, turn, att, new Requirement(buildingCost, costRes));
                if (isSpecial)
                {
                    //DO SOMETHING FOR SPECIAL BUILDING
                    switch (id) {
                        case BuildingType.Carry:
                            break;
                    }
                }
                _buildings.Add(id, build);
            }
        }

        private bool IsSpecial(XElement child)
        {
            var firstOrDefault = child.Descendants().FirstOrDefault(n => n.Name == "IsSpecial");
            if (firstOrDefault != null)
            {
                return bool.Parse(firstOrDefault.Value);
            }
            return false;
        }

        private IEnumerable<BuildingType> GetBuilding(XElement child)
        {
            List<BuildingType> res = new List<BuildingType>();
            var firstOrDefault = child.Descendants().FirstOrDefault(n => n.Name == "Builds");
            if (firstOrDefault != null)
            {
                var attributes = firstOrDefault.Descendants();
                foreach (XElement attribute in attributes)
                {
                    res.Add((BuildingType)int.Parse(attribute.Value));
                }
            }
            return res;
        }

        private Resources GetRessource(XElement child,string attName)
        {
            int gold=0, meat=0, pop=0, wood =0, rock = 0;
            var firstOrDefault = child.Descendants().FirstOrDefault(n => n.Name == attName);
            if (firstOrDefault != null)
            {
                var attributes = firstOrDefault.Descendants();
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
            return new Resources(wood,gold,meat,rock,pop);
        }

        private string GetAttribute(XElement child, string att)
        {
            var firstOrDefault = child.Attributes().FirstOrDefault(n => n.Name == att);
            if (firstOrDefault != null)
            {
                return firstOrDefault.Value;
            }
            return "";
        }

        public static BuildingLoader getInstance()
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
