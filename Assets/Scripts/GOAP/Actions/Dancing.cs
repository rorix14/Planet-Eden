namespace RPG.GOAP
{
    public class Dancing : GAction
    {
        public override bool PrePerform()
        {
            target = GWorld.Instance.GetQueue(PlaceOfInterestType.DanceArea).RemoveResourse();
            if (!target) return false;
            GWorld.Instance.GetWorld().ModifyState(States.freeDanceArea, -1);
            return true;
        }

        public override bool PostPerform()
        {
            if (!target) return true;

            agentBeliefs.RemoveState(States.mustDance);
            GWorld.Instance.GetQueue(PlaceOfInterestType.DanceArea).AddResourse(target);
            GWorld.Instance.GetWorld().ModifyState(States.freeDanceArea, 1);
            target = null;

            return true;
        }
    }
}