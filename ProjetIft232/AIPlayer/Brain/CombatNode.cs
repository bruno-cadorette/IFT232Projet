using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AIPlayer.Brain
{
    class CombatNode : ActionNode
    {
        public CombatNode(PlayerAI p) : base(p)
        {
        }

        public override BuildableEntity MakeDecision(City city)
        {
            throw new NotImplementedException();
        }
    }
}
