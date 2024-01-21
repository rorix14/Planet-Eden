namespace RPG.GOAP
{
    public class Sleep : GAction
    {
        public override bool PrePerform()
        {
            target = gameObject;
            return true;
        }

        public override bool PostPerform()
        {
            return true;
        }
    }
}