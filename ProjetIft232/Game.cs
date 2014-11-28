using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjetIft232.Buildings;
using ProjetIft232.Technologies;
using System.Runtime.Serialization;
using System.IO;

namespace ProjetIft232
{
    [DataContract]
    public class Game
    {
        [DataMember]
        public List<Player> Players { get; set; }

        public Player CurrentPlayer
        {
            get
            {
                return Players[PlayerIndex];
            }
        }

        private int _playerIndex = 0;
        [DataMember]
        private int Hostility { get; set; }
        [DataMember]
        private int Alea { get; set; }
        [DataMember]
        public int PlayerIndex { get; private set; }

        //Variable global, à changer 
        public static int TourIndex { get; private set; }

        RandomEvent evt = new RandomEvent();

        public Game()
        {
            BuildingLoader.GetInstance();
            Players = new List<Player>();
            Hostility = 3;
            Alea = 0;
        }

        public void Save(string fileName)
        {
            using (var fileStream = File.Create(fileName))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(Game));
                serializer.WriteObject(fileStream, this);
            }
        }

        public static Game Load(string fileName)
        {
            using (var fileStream = File.Open(fileName,FileMode.Open))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(Game));
                fileStream.Position = 0;
                return serializer.ReadObject(fileStream) as Game;
            }
        }

        public string NextTurn()
        {
            string turnText = "";
            int win = 0;
            do
            {      
                turnText = NextTurnInternal();
                Console.WriteLine(turnText);
                if (CurrentPlayer is PlayerAI)
                {
                    Console.WriteLine("\nTour du joueur AI: {0}", CurrentPlayer.playerName);
                    ((PlayerAI)CurrentPlayer).Play();

                }
            }
            while (CurrentPlayer is PlayerAI);
            win = ifWin();
            if (win == 0)
            {
                win = ifLose();
            }
            if (win == 1)
            {
                return "Vous avez gagné!";
            }

            if (win == -1)
            {
                return "Vous avez perdu!  :'( ";
            }
            return turnText;
        }

        private string NextTurnInternal()
        {
            string turnText = "";
            CurrentPlayer.Cities.ForEach(n => n.Update());
            CurrentPlayer.ResearchedTech.ToList().ForEach(n => n.Update());
            Random hostylityAument = new Random();
            Hostility += hostylityAument.Next(-1, 1 + TourIndex / 10);
            foreach (City city in CurrentPlayer.Cities)
            {

                if (Hostility > hostylityAument.Next(0, 100))
                {
                    turnText += city.Attack(BarbarianArmyGenerator.CreateArmy(TourIndex));

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
                TourIndex++;
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
            else
            {
                if (CurrentPlayer.Cities.Any(n => n.Ressources >= City.CostToCreate))
                {
                    CurrentPlayer.Cities.First(n => n.Ressources >= City.CostToCreate).RemoveResources(City.CostToCreate);
                    CurrentPlayer.CreateCity(name);
                    return true;
                }
            }
            return false;
        }

        public void ChangeCityFocus()
        {
            CurrentPlayer.NextCity();
        }

        public bool Transfer(string nomville, int bois, int or, int viande, int rock, int pop)
        {
            City city = CurrentPlayer.Cities.FirstOrDefault(n => n.Name == nomville);
            if (city != null)
            {
                return CurrentPlayer.CurrentCity.TransferResources(city, new Resources(bois, or, viande, rock, pop));
            }
            else
            {
                return false;
            }
        }

        // Méthode getMarket pour récupérer une instance de Market (construit) si elle existe

        public Building getMarket()
        {
            Building m = CurrentPlayer.CurrentCity.FinishedBuildings.FirstOrDefault(n => n is Market);

            return m;

        }

        public bool ResearchTechnology(int type)
        {
            Technology technology = TechnologyFactory.CreateTechnology(type, CurrentPlayer.CurrentCity);
            if (technology == null)
            {
                return false;
            }
            else
            {
                CurrentPlayer.ResearchedTech.Add(technology);
                CurrentPlayer.CurrentCity.RemoveResources(technology.Requirement.Resources);
                return true;
            }
        }

        private int ifWin()
        {
            int population=0;
            foreach (var city in CurrentPlayer.Cities){
                population += city.Ressources.get("Population");
            }
            if (population >= 10000)
            {
                return 1;
            }
            return 0;
        }

        private int ifLose()
        {
            int population=0;
            foreach (var city in CurrentPlayer.Cities){
                population += city.Ressources.get("Population");
            }
            if (population <= 0)
            {
                return -1;
            }
            if (Game.TourIndex >= 150)
            {
                return -1;
            }
            return 0;
        }
        
    }
}

