using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AIPlayer.Brain
{
    public abstract  class ActionNode
    {
        protected PlayerAI _player;

        public ActionNode(PlayerAI p)
        {
            _player = p;
        }

        public abstract BuildableEntity MakeDecision(City city);



    }
}
