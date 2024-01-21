namespace RPG.GOAP
{
    public class Rest : GAction
    {
        public override bool PrePerform()
        {
            target = GWorld.Instance.GetQueue(PlaceOfInterestType.RestArea).RemoveResourse();
            if (!target) return false;
            GWorld.Instance.GetWorld().ModifyState(States.freeRestPlace, -1);
            return true;
        }

        public override bool PostPerform()
        {
            if (!target) return true;

            agentBeliefs.RemoveState(States.exhausted);
            GWorld.Instance.GetQueue(PlaceOfInterestType.RestArea).AddResourse(target);
            GWorld.Instance.GetWorld().ModifyState(States.freeRestPlace, 1);
            target = null;

            return true;
        }
    }
}