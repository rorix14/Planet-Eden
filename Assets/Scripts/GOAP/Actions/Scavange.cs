namespace RPG.GOAP
{
    public class Scavange : GAction
    {
        public override bool PrePerform()
        {
            target = GWorld.Instance.GetQueue(PlaceOfInterestType.ScavengeSpot).RemoveResourse();
            if (!target) return false;

            GWorld.Instance.GetWorld().ModifyState(States.freeScavangeSpot, -1);
            return true;
        }

        public override bool PostPerform()
        {
            if (!target) return true;

            GWorld.Instance.GetQueue(PlaceOfInterestType.ScavengeSpot).AddResourse(target);
            GWorld.Instance.GetWorld().ModifyState(States.freeScavangeSpot, 1);
            target = null;

            return true;
        }
    }
}

