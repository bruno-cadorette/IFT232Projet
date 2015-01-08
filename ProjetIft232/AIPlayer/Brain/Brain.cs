using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Technologies;

namespace Core.AIPlayer.Brain
{
    enum Priority
    {
        NeedSoldiers = 0,
        Tech = 1,
        Resources = 2,
        Else = 3
    }
    public class Brain
    {
        private CombatNode combatNode;
        private EconomyNode economyNode;
        private PlayerAI player;

        public Brain(PlayerAI p)
        {
            player = p;
            combatNode = new CombatNode(p);
            economyNode = new EconomyNode(p);
        }
        public Tuple<int, BuildableEntity> NextThingToDo(City city)
        {
            Priority prio = Priority.NeedSoldiers;
            //Is there bad people near?
            BuildableEntity entity = combatNode.MakeDecision(city);
            //Can we afford a Tech?
            //Are we low on resources
            if (entity == null)
            {
                entity = economyNode.MakeDecision(city);
                if(entity is Technology)
                    prio = Priority.Tech;
                else
                {
                    prio = Priority.Resources;
                }

            }

            //Nothing can be done
            return entity == null ?  null : new Tuple<int, BuildableEntity>((int)prio,entity);
        }
    }
}
