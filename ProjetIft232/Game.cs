using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjetIft232
{
    class Game
    {
        static public Game CurrentGame;
        public int MyProperty { get; set; }
        public List<IPlayer> Players { get; set; }
        public event EventHandler<TurnEventArgs> TurnBegin;
        public event EventHandler<TurnEventArgs> TurnEnd;

        public IPlayer CurrentPlayer { 
            get 
            {
                return Players[PlayerIndex];
            }
        }

        private int _playerIndex = 0;
        public int PlayerIndex 
        { 
            get
            {
                return _playerIndex;
            }
            private set
            {
                if (_playerIndex != value)
                {
                    TurnEnd(this, new TurnEventArgs(Players[_playerIndex]));
                    _playerIndex = value % Players.Count;
                    TurnBegin(this, new TurnEventArgs(Players[_playerIndex]));
                }
            }
        }

        public Game()
        {
            Players = new List<IPlayer>();
            Players.Add(new Player());
        }

        public CommandResult NextTurn()
        {
            PlayerIndex++; ;
            return new CommandResult("Changement de tour.");
        }
    }
}
