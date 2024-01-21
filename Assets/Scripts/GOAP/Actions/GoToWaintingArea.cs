namespace RPG.GOAP
{
    public class GoToWaintingArea : GAction
    {
        public override bool PrePerform()
        {
            target = GWorld.Instance.GetQueue(PlaceOfInterestType.WaintingArea).RemoveResourse();
            if (!target) return false;

            GWorld.Instance.GetWorld().ModifyState(States.freeWaintingSpace, -1);
            inventory.AddItem(target);

            GWorld.Instance.GetQueue(PlaceOfInterestType.WaitingCollectors).AddResourse(gameObject);
            return true;
        }

        public override bool PostPerform()
        {
            agentBeliefs.RemoveState(States.Wait);
            return true;
        }
    }
}