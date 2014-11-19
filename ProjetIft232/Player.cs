using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace ProjetIft232
{
    public class Player
    {
        public List<City> Cities { get; set; }

        public Player()
        {
            Cities = new List<City>();
            _indexCity = 0;
        }

        public City CurrentCity { get; private set; }
        private int _indexCity;

        public void NextCity()
        {
            _indexCity = (_indexCity + 1)%Cities.Count;
            CurrentCity = Cities[_indexCity];
        }

        public void CreateCity(string name)
        {
            Cities.Add(new City(name));
        }


        //Deprecated
        public City GetCity()
        {
            return CurrentCity;
        }


        public override string ToString()
        {
            return "Joueur 1";
        }

        public void WriteXML()
        {

            //Buildings.Building overview = new Buildings.Building(Buildings.BuildingType.Mine, "toto", "Blablabla", 3, new Resources(0,0,0,0,0), Buildings.Requirement.Zero());

            int[] resourcesTable = new int[5];

            resourcesTable[0] = Cities.First().Ressources.get("Wood");
            resourcesTable[1] = Cities.First().Ressources.get("Gold");
            resourcesTable[2] = Cities.First().Ressources.get("Meat");
            resourcesTable[3] = Cities.First().Ressources.get("Rock");
            resourcesTable[4] = Cities.First().Ressources.get("Population");

            List<Buildings.Building> bulle = Cities.First().Buildings;
            
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
