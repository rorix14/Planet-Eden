namespace RPG.GOAP
{
    public class Terrified : GAction
    {
        public override bool PrePerform()
        {
            target = gameObject;
            return true;
        }

        public override bool PostPerform()
        {
            if (agentBeliefs.HasState(States.isTerrified)) return false;
            return true;
        }       
    }
}

