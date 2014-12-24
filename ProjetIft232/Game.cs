using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Core.Buildings;
using Core.Technologies;
using Core.Utility;

namespace Core
{
    [DataContract]
    public class Game
    {
        private RandomEvent evt = new RandomEvent();

        public Game()
        {
            BuildingFactory.GetInstance();
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
            get { return Players[PlayerIndex]; }
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
                g.evt = new RandomEvent();
                return g;
            }
        }

        public IEnumerable<string> NextTurn()
        {
            List<string> turnText = new List<string>();
            do
            {
                turnText.Add(NextTurnInternal());
                //Console.WriteLine(turnText);
                if (CurrentPlayer is PlayerAI)
                {
                    Console.WriteLine("\nTour du joueur AI: {0}", CurrentPlayer.playerName);
                    ((PlayerAI) CurrentPlayer).Play();
                }
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

        private string NextTurnInternal()
        {
            string turnText = "";
            CurrentPlayer.Cities.ForEach(n => n.Update());
            CurrentPlayer.ResearchedTech.ToList().ForEach(n => n.Update());
            Random hostylityAument = RandomGen.GetInstance();

            Hostility += hostylityAument.Next(-1, 1 + TurnIndex/10);
            foreach (City city in CurrentPlayer.Cities)
            {
                if (Hostility > hostylityAument.Next(0, 100))
                {
                    turnText += city.Attack(BarbarianArmyGenerator.CreateArmy(TurnIndex));
                }
                //On reutilise la variable hostilityAument pour generer les evenements aleatoires
                Alea = hostylityAument.Next(0, 100);
                if (Alea > 90)
                {
                    turnText += evt.Next(CurrentPlayer.Cities.First());
                }
            }

            PlayerIndex++;
            if (PlayerIndex >= Players.Count)
            {
                PlayerIndex -= Players.Count;
                TurnIndex++;
            }
            return turnText;
        }

        public void ApplyTech(UpgradableEntity entity, Technology technology)
        {
            entity.Upgrade(technology);
            CurrentPlayer.CurrentCity.RemoveResources(technology.ApplicationCost);
        }


        public bool CreateCity(string name)
        {
            if (CurrentPlayer.CurrentCity.Ressources >= City.CostToCreate)
            {
                CurrentPlayer.CurrentCity.RemoveResources(City.CostToCreate);
                CurrentPlayer.CreateCity(name);
                return true;
            }
            if (CurrentPlayer.Cities.Any(n => n.Ressources >= City.CostToCreate))
            {
                CurrentPlayer.Cities.First(n => n.Ressources >= City.CostToCreate).RemoveResources(City.CostToCreate);
                CurrentPlayer.CreateCity(name);
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
            var population = CurrentPlayer.Cities.Sum(city => city.Ressources[ResourcesType.Population]);
            
            
            
            return population >= 10000;
        }

        public Boolean HasLost()
        {
            var population = CurrentPlayer.Cities.Sum(city => city.Ressources[ResourcesType.Population]);
            return population <= 0 || TurnIndex >= 150;
        }
    }
}