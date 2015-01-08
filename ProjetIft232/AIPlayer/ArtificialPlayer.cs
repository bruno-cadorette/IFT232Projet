using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utility;

namespace Core.AIPlayer
{
    enum EmotionalState
    {
        Happy = 0,
        Scared = 1,
        Smart = 2
    }
    /*
     * First, we need to make priority, what's important
     * Then, we do the most important things first(weighted queue could be nice)
     * 1- Direct Threat, barbarian, army, zombie -> Make army/ casern/ whatever we need to survive
     * 2- Researched a Tech
     * 3- Build Resources Building according to what we need/ not have
     * 4- Build others buildings, like Market, School
     * However, if we do not have the requirement, we want to build the requirement, if we need soldiers, but we don't have a Casern, we build one
     * State Pattern for comportement, Scared, happy, adventurous, warned
     * the comportement will affect how the Brain work and what is built. 
     */
    class ArtificialPlayer : Player
    {
        //Brain
        private Brain.Brain brain;
        //City
        
        //EmotionalState
        private EmotionalState emotionalState;
        //Queue of next thingg to do
        WeightedQueue<BuildableEntity> NextThingToDo { get; set; }

        public void MakeTurn()
        {
            foreach (var city in Cities)
            {
               var thing = brain.NextThingToDo(city);
               while (thing != null)
                {
                    NextThingToDo.Queue(thing.Item1, thing.Item2);
                    thing = brain.NextThingToDo(city);
                }
            }
            //Move Unit
        }
    }
}
