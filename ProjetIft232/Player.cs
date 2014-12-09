using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using ProjetIft232.Technologies;

namespace ProjetIft232
{
    [DataContract]
    public class Player
    {
        public Player()
        {
            ResearchedTech = new ObservableCollection<Technology>();
            Cities = new List<City>();
            _indexCity = 0;
        }

        [DataMember]
        public ObservableCollection<Technology> ResearchedTech { get; private set; }

        [DataMember]
        public List<City> Cities { get; set; }

        [DataMember]
        public string playerName { get; set; }

        public City CurrentCity
        {
            get { return Cities[_indexCity]; }
        }

        [DataMember]
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
            return TechnologyFactory.GetInstance().Technologies()
                .Where(n => ResearchedTech.All(m => n.ID != m.ID && !m.InConstruction));
        }

        public bool ResearchTechnology(int type)
        {
            Technology technology = TechnologyFactory.CreateTechnology(type, CurrentCity.Ressources,
                CurrentCity.Buildings, ResearchedTech);
            if (technology == null)
            {
                return false;
            }
            ResearchedTech.Add(technology);
            CurrentCity.RemoveResources(technology.Requirement.Resources);
            return true;
        }
    }
}