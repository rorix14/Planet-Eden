
public class Rest : GAction
{
    public override bool PrePerform()
    {
        target = GWorld.Instance.GetQueue(PlaceOfInterestType.Lounge).RemoveResourse();
        if (!target) return false;
        GWorld.Instance.GetWorld().ModifyState(States.freeRestPlace, -1);
        return true;
    }

    public override bool PostPerform()
    {
        agentBeliefs.RemoveState(States.exhausted);
        GWorld.Instance.GetQueue(PlaceOfInterestType.Lounge).AddResourse(target);
        GWorld.Instance.GetWorld().ModifyState(States.freeRestPlace, 1);
        return true;
    } 
}
