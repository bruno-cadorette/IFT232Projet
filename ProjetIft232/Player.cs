using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjetIft232
{
    class Player : IPlayer
    {
        public List<City> Cities { get; set; }

        public event EventHandler<TurnEventArgs> PlayerTurnBegin;
        public event EventHandler<TurnEventArgs> PlayerTurnEnd;

        public Player()
        {
            Cities = new List<City>();
            Cities.Add(new City());
            Game.CurrentGame.TurnBegin += TurnBegin;
            Game.CurrentGame.TurnEnd += TurnEnd;
        }

        private void TurnBegin(object sender, TurnEventArgs args)
        {
            if (args.PlayersTurn == this)
            {
                PlayerTurnBegin(sender, args);
            }
        }

        private void TurnEnd(object sender, TurnEventArgs e)
        {
            if (e.PlayersTurn == this)
            {
                PlayerTurnEnd(sender, e);
            }
        }


        [UserCallable("ville")]
        public City GetCity()
        {
            return Cities.First();
        }

        [UserCallable("Next")]
        public CommandResult NextTurn()
        {
            return Game.CurrentGame.NextTurn();
        }
    }
}
