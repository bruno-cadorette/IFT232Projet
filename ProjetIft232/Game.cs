using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Core.Buildings;
using Core.Technologies;
using Core.Utility;
using Core.Military;
using Core.Map;
using Core.RandomEvent;

namespace Core
{
    [DataContract]
    public class Game
    {
        private RandomEventFactory evt = new RandomEventFactory();

        public WorldMap WorldMap { get; private set; }

        public Game()
        {
            WorldMap = new WorldMap();
            Players = new List<Player>();
            Hostility = 3;
            Alea = 0;
        }

        [DataMember]
        public List<Player> Players { get; set; }


        [DataMember]
        private int Hostility { get; set; }

        [DataMember]
        private int Alea { get; set; }

        [DataMember]
        public int PlayerIndex { get; private set; }


        public Player CurrentPlayer
        {
            get { return Players.ElementAtOrDefault(PlayerIndex); }
        }

         
        [DataMember]
        public int TurnIndex { get; private set; }


        public void Save(string fileName)
        {
            using (var fileStream = File.Create(fileName))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof (Game));
                serializer.WriteObject(fileStream, this);
            }
        }

        public static Game Load(string fileName)
        {
            using (var fileStream = File.Open(fileName, FileMode.Open))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof (Game));
                fileStream.Position = 0;
                Game g = serializer.ReadObject(fileStream) as Game;
                g.evt = new RandomEventFactory();
                return g;
            }
        }

        public IEnumerable<string> NextTurn()
        {
            List<string> turnText = new List<string>();
            WorldMap.Update();
            do
            {
                CurrentPlayer.Cities = WorldMap.Where(x => x.Value.PlayerId == CurrentPlayer.ID && x.Value is City).Select(x => x.Value as City).ToList();
                CurrentPlayer.Cities.ForEach(n => n.Update());
                CurrentPlayer.ResearchedTech.ToList().ForEach(n => n.Update());
                turnText.Add(IncreaseHostility());
                if (CurrentPlayer.HasLost())
                {
                    Players.Remove(CurrentPlayer);
                }
                else if (CurrentPlayer is PlayerAI)
                {
                    turnText.Add(String.Format("Tour du joueur AI: {0}", CurrentPlayer.playerName));
                    (CurrentPlayer as PlayerAI).Play();
                }
                NextPlayer();
            } while (CurrentPlayer is PlayerAI);
            if (HasWin())
            {
                turnText.Add("Vous avez gagné!");
            }
            if (HasLost())
            {
                turnText.Add("Vous avez perdu!  :'( ");
            }           
            return turnText.Where(t=>!string.IsNullOrEmpty(t));
        }
        private void NextPlayer()
        {
            PlayerIndex++;
            if (PlayerIndex >= Players.Count)
            {
                PlayerIndex -= Players.Count;
                TurnIndex++;
            }
        }

        private string IncreaseHostility()
        {
            string turnText="";
            Random hostylityAument = RandomGen.GetInstance();

            Hostility += hostylityAument.Next(-1, 1 + TurnIndex / 10);
            foreach (City city in CurrentPlayer.Cities)
            {
                if (Hostility > hostylityAument.Next(0, 100))
                {
                    WorldMap.AddToRandomPosition(BarbarianArmy.CreateArmy(TurnIndex));
                }
                //On reutilise la variable hostilityAument pour generer les evenements aleatoires
                Alea = hostylityAument.Next(0, 100);
                if (Alea > 90)
                {
                    turnText += evt.Next(CurrentPlayer.Cities.First());
                }
            }
            return turnText;
        }

        public void ApplyTech(UpgradableEntity entity, Technology technology)
        {
            entity.Upgrade(technology);
            CurrentPlayer.CurrentCity.RemoveResources(technology.ApplicationCost);
        }
        public void CreatePlayer(string playerName, string cityName)
        {
            Player player = new Player()
            {
                ID = Players.Count,
                playerName = playerName,
            };

            WorldMap.AddToRandomPosition(player.CreateCity(cityName));
            player.NextCity();
            Players.Add(player);
        }

        public bool CreateCity(string name)
        {
            var city = CurrentPlayer.Cities.FirstOrDefault(n => n.Ressources >= City.CostToCreate);
            if (city!=null)
            {
                city.RemoveResources(City.CostToCreate);
                WorldMap.AddToRandomPosition(CurrentPlayer.CreateCity(name));
                return true;
            }
            return false;
        }

        public void ChangeCityFocus()
        {
            CurrentPlayer.NextCity();
        }

        public bool Transfer(string nomville, Resources resources)
        {
            City city = CurrentPlayer.Cities.FirstOrDefault(n => n.Name == nomville);
            if (city != null)
            {
                return CurrentPlayer.CurrentCity.TransferResources(city, resources);
            }
            return false;
        }

        // Méthode getMarket pour récupérer une instance de Market (construit) si elle existe

        public Building GetMarket()
        {
            return CurrentPlayer.CurrentCity.FinishedBuildings.FirstOrDefault(n => n is Market);
        }

        public Boolean HasWin()
        {
            return !HasLost() && CurrentPlayer.Cities.Sum(city => city.Ressources[ResourcesType.Population]) >= 10000;
        }

        public Boolean HasLost()
        {
            return Players.Count == 0;
        }
    }
}