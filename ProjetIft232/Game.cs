using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjetIft232.Buildings;

namespace ProjetIft232
{
    class Game
    {
        
        //static public Game CurrentGame;
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
            BuildingLoader.getInstance();
            Players = new List<Player>();
            Hostility = 3;
            Alea = 0;
        }

        public string NextTurn()
        {
            string resumerDuTour = "";
            CurrentPlayer.Cities.First().Update();

            Random hostylityAument = new Random();
            Hostility +=hostylityAument.Next(-1, 1+TourIndex/10);
            if (Hostility > hostylityAument.Next(0, 100))
            {
               resumerDuTour += CurrentPlayer.Cities.First().Attack(BarbarianArmyGenerator.CreateArmy(TourIndex));

            }
            //On reutilise la variable hostilityAument pour generer les evenements aleatoires
            Alea = hostylityAument.Next(0,100);
            if (Alea > 90)
            {
                resumerDuTour += evt.Next(CurrentPlayer.Cities.First());

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
    }
}
