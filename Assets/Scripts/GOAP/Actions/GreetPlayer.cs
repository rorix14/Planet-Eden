namespace RPG.GOAP
{
    public class GreetPlayer : GAction
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
