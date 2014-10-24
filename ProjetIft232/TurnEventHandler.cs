using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjetIft232
{
    class TurnEventArgs : EventArgs
    {
        public IPlayer PlayersTurn { get; private set; }
        public TurnEventArgs(IPlayer Whom)
        {
            PlayersTurn = Whom;
        }
    }
}
