using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public int PlayerIndex { get; private set; }
        public static int TourIndex { get; private set; }

        public Game()
        {
            Players = new List<Player>();
        }

        [UserCallable("Next")]
        public CommandResult NextTurn()
        {
            CurrentPlayer.Cities.First().Update();
            PlayerIndex++;
            
           //Sinon c'est attardé
            if (PlayerIndex + 1 > Players.Count)
            {
                PlayerIndex-=Players.Count;
                TourIndex++;
            }
            return new CommandResult("Changement de tour.");
        }
    }
}
