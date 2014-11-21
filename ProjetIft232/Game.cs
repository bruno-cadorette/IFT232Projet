using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjetIft232.Buildings;

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

        public string NextTurn()
        {
            string resumerDuTour = "";
            CurrentPlayer.Cities.ForEach(n => n.Update());
            Random hostylityAument = new Random();
            Hostility += hostylityAument.Next(-1, 1 + TourIndex/10);
            foreach (City city in CurrentPlayer.Cities)
            {

                if (Hostility > hostylityAument.Next(0, 100))
                {
                    resumerDuTour += city.Attack(BarbarianArmyGenerator.CreateArmy(TourIndex));

                }
                //On reutilise la variable hostilityAument pour generer les evenements aleatoires
                Alea = hostylityAument.Next(0, 100);
                if (Alea > 90)
                {
                    resumerDuTour += evt.Next(CurrentPlayer.Cities.First());

                }
            }

            PlayerIndex++;
           //Sinon c'est attardé
            if (PlayerIndex + 1 > Players.Count)
            {
                PlayerIndex-=Players.Count;
                TourIndex++;
            }
            return resumerDuTour;
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
                Building m = CurrentPlayer.CurrentCity.Buildings.FirstOrDefault(n => (n.Name == "Marché") && (n.InConstruction == false)) ;

                return m  ;
                
            }
            
        }
    }

