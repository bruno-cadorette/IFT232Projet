using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjetIft232.Buildings;
using ProjetIft232.Technologies;

namespace ProjetIft232
{
    public class Game
    {
        
        //static public Game CurrentGame
        public List<Player> Players { get; set; }

        public Player CurrentPlayer 
        { 
            get 
            {
                return Players[PlayerIndex];
            }
        }

        private int _playerIndex = 0;
        private int Hostility { get;  set; }
        private int Alea { get; set; }
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

        public void NextTurn()
        {
            do
            {
                 Console.WriteLine(NextTurnInternal());
                if (CurrentPlayer is PlayerAI)
                {
                    Console.WriteLine("\nTour du joueur AI: {0}", CurrentPlayer.playerName);
                    ((PlayerAI)CurrentPlayer).Play();
                }
            }
            while (CurrentPlayer is PlayerAI);
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

        public bool ApplyTech(string name)
        {
            Technology tech = CurrentPlayer.ResearchedTech.FirstOrDefault(n => n.Name == name);
            if (tech == null) return false;
            Building build = CurrentPlayer.CurrentCity.FindBuildingForReschearded(tech);
            if (build == null) return false;
            City city = CurrentPlayer.CurrentCity;
            return BuildingFactory.UpgrateBuilding(ref build,tech,ref city);
            
        }

        public  bool CreateCity(string name)
        {
            if (CurrentPlayer.CurrentCity.Ressources > City.CostToCreate)
            {
                CurrentPlayer.CurrentCity.RemoveResources(City.CostToCreate);
                CurrentPlayer.CreateCity(name);
                return true;
            }
            else
            {
                if (CurrentPlayer.Cities.Any(n => n.Ressources > City.CostToCreate))
                {
                    CurrentPlayer.Cities.First(n => n.Ressources > City.CostToCreate).RemoveResources(City.CostToCreate);
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
                return CurrentPlayer.CurrentCity.TransferResources(city, new Resources(bois,or,viande,rock,pop));
            }
            else
            {
                return false;
            }
        }

            // Méthode getMarket pour récupérer une instance de Market (construit) si elle existe

            public Building getMarket()
            {
                Building m = CurrentPlayer.CurrentCity.Buildings.FirstOrDefault(n => (n is Market) && (n.InConstruction == false)) ;

                return m  ;
                
            }

        public bool ReshearchedTech(Technology choix)
        {
            Technology tech = TechnologyFactory.ReshearcheTech(CurrentPlayer.CurrentCity,
                choix.Name);
            if (tech == null)
            {
                return false;
            }
            CurrentPlayer.ResearchedTech.Add(tech);
            return true;
        }
    }
    }

