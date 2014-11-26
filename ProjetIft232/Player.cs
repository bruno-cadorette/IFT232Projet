using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using ProjetIft232.Technologies;

namespace ProjetIft232
{
    public class Player
    {
        public ObservableCollection<Technology> ResearchedTech { get; private set; } 
        public List<City> Cities { get; set; }
        public string playerName { get; set; }

        public Player()
        {
            ResearchedTech = new ObservableCollection<Technology>();
            Cities = new List<City>();
            _indexCity = 0;
        }

        public City CurrentCity { 
            get {
                return Cities[_indexCity];
            }
        }

        public int _indexCity { get; private set; }

        public void NextCity()
        {
            _indexCity = (_indexCity + 1)%Cities.Count;
        }

        public void CreateCity(string name)
        {
            City city = new City(name);
            city.ResearchedTechnologies.AddRange(ResearchedTech.ToList());
            ResearchedTech.CollectionChanged += city.TechChanged;
            Cities.Add(city);
        }

        public override string ToString()
        {
            return playerName;
        }

        public IEnumerable<Technology> GetTechnologies()
        {
            return TechnologyLoader.GetInstance().Technologies()
                .Where(n => ResearchedTech.All(m => n.ID != m.ID && !m.InConstruction));

        }

        public bool ResearchTechnology(int type)
        {
            Technology technology = TechnologyFactory.ResearchTechnology(type, CurrentCity);
            if (technology == null)
            {
                return false;
            }
            else
            {
                ResearchedTech.Add(technology);
                CurrentCity.RemoveResources(technology.Requirement.Resources);
                return true;
            }
        }

        public void WriteXML()
        {

            //Buildings.Building overview = new Buildings.Building(Buildings.2, "toto", "Blablabla", 3, new Resources(0,0,0,0,0), Buildings.Requirement.Zero());

            int[] resourcesTable = new int[5];

            resourcesTable[0] = Cities.First().Ressources.get("Wood");
            resourcesTable[1] = Cities.First().Ressources.get("Gold");
            resourcesTable[2] = Cities.First().Ressources.get("Meat");
            resourcesTable[3] = Cities.First().Ressources.get("Rock");
            resourcesTable[4] = Cities.First().Ressources.get("Population");

            List<Buildings.Building> bulle = Cities.First().Buildings.ToList();
            
            //Il prend un Army Warrior au lieu de prendre un ArmyUnit
            //List<Army.ArmyUnit> chevre = Cities.First().Army;

            System.Xml.Serialization.XmlSerializer writer1 =
                new System.Xml.Serialization.XmlSerializer(typeof(int[]));
            System.Xml.Serialization.XmlSerializer writer2 =
                new System.Xml.Serialization.XmlSerializer(typeof(List<Buildings.Building>));
            //System.Xml.Serialization.XmlSerializer writer3 =
                //new System.Xml.Serialization.XmlSerializer(typeof(List<Army.ArmyUnit>));

            var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineOnAttributes = true;
            settings.OmitXmlDeclaration = true;

            System.IO.StreamWriter file = new System.IO.StreamWriter(
                @".\Testing.xml");

            var writer4 = XmlWriter.Create(file, settings);
            var writer5 = XmlWriter.Create(file, settings);
            //var writer6 = XmlWriter.Create(file, settings);

            writer1.Serialize(writer4, resourcesTable, emptyNs);
            writer2.Serialize(writer5, bulle, emptyNs);
            //writer3.Serialize(writer6, chevre, emptyNs);


            file.Close();
        }
    }
}
