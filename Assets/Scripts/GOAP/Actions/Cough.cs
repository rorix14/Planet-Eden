namespace RPG.GOAP
{
    public class Cough : GAction
    {
        public override bool PrePerform()
        {
            target = gameObject;
            return true;
        }

        public override bool PostPerform()
        {
            agentBeliefs.RemoveState(States.needsToCough);
            return true;
        }  
    }
}
