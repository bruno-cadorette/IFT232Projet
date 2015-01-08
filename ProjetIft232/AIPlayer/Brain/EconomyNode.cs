using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Buildings;

namespace Core.AIPlayer.Brain
{
    class EconomyNode : ActionNode
    {
        public EconomyNode(PlayerAI p) : base(p)
        {
        }

        public override BuildableEntity MakeDecision(City city)
        {
            BuildableEntity entity = null;
            //Tech
            foreach (var tech in _player.TechLeftToDo())
            {
                if (tech.CanBeBuild(city))
                {
                    entity = tech;
                    break;
                }
            }
            //Resource
            ResourcesType lowestType = city.GetBuildingsResources().GetLowest();
            var buildingDoingThatResource = BuildingFactory.GetInstance().GetBuildingDoingThatResources(lowestType);

            return entity;
        }
    }
}
