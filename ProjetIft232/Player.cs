using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using Core.Technologies;

namespace Core
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
        public int ID { get; set; }

        [DataMember]
        public ObservableCollection<Technology> ResearchedTech { get; private set; }

        [DataMember]
        public List<City> Cities { get; set; }

        [DataMember]
        public string playerName { get; set; }

        public City CurrentCity
        {
            get { return Cities.ElementAtOrDefault(_indexCity); }
        }

        [DataMember]
        public int _indexCity { get; private set; }

        public void NextCity()
        {
            _indexCity = (_indexCity + 1)%Cities.Count;
        }

        public City CreateCity(string name)
        {
            City city = new City(name);
            city.PlayerId = ID;
            city.ResearchedTechnologies.AddRange(ResearchedTech);
            ResearchedTech.CollectionChanged += city.TechChanged;
            Cities.Add(city);
            return city;
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
        public bool HasLost()
        {
            return Cities.Count == 0;
        }

        public IEnumerable<Technology> TechLeftToDo()
        {
            bool isDone = false;
            foreach (var each in TechnologyFactory.GetInstance().Technologies())
            {
                foreach (var each2 in ResearchedTech)
                {
                    if (each.ID == each2.ID)
                    {
                        isDone = true;
                    }
                }
                if (!isDone)
                {
                    yield return each;
                }
                isDone = false;
            }
        }
    }
}