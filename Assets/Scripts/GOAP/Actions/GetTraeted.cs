namespace RPG.GOAP
{
    public class GetTraeted : GAction
    {
        public override bool PrePerform()
        {
            target = transform.gameObject;
            GWorld.Instance.GetQueue(PlaceOfInterestType.sickPatient).AddResourse(transform.gameObject);
            GWorld.Instance.GetWorld().ModifyState(States.sickPatients, 1);
            return true;
        }

        public override bool PostPerform()
        {
            if (agentBeliefs.HasState(States.isSick)) return false;

            return true;
        }
    }
}