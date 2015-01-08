using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AIPlayer.Brain
{
    //This node get Info from what it sees, it is called by Nodes and will tell them what it sees
    class WatcherNode
    {
        private PlayerAI _player;

        public WatcherNode(PlayerAI p)
        {
            _player = p;
        }

        public bool IsEnnemyNear(City city)
        {
            return false;
        }

        public bool IsArmyInSight()
        {
            return false;
        }
    }
}
