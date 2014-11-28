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
using System.Runtime.Serialization;

namespace ProjetIft232
{
    [DataContract]
    public class Player
    {
        [DataMember]
        public ObservableCollection<Technology> ResearchedTech { get; private set; }

        [DataMember]
        public List<City> Cities { get; set; }

        [DataMember]
        public string playerName { get; set; }

        public Player()
        {
            ResearchedTech = new ObservableCollection<Technology>();
            Cities = new List<City>();
            _indexCity = 0;
        }

        public City CurrentCity
        {
            get
            {
                return Cities[_indexCity];
            }
        }

        [DataMember]
        public int _indexCity { get; private set; }

        public void NextCity()
        {
            _indexCity = (_indexCity + 1) % Cities.Count;
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
            Technology technology = TechnologyFactory.CreateTechnology(type, CurrentCity);
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
    }
}
