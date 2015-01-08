namespace Core.AIPlayer.Brain
{
    public class ImprovmentNode : ActionNode
    {
        //This Node care about the building/Tech we need for Requirement, for exemple, we need Casern for Warrior 
        public ImprovmentNode(PlayerAI p) : base(p)
        {
        }

        public override BuildableEntity MakeDecision(City city)
        {
            throw new System.NotImplementedException();
        }
    }
}