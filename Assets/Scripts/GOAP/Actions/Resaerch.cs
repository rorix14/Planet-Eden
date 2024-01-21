namespace RPG.GOAP
{
    public class Resaerch : GAction
    {
        public override bool PrePerform()
        {
            target = GWorld.Instance.GetQueue(PlaceOfInterestType.researchArea).RemoveResourse();

            if (!target) return false;

            GWorld.Instance.GetWorld().ModifyState(States.freeResearchArea, -1);
            return true;
        }

        public override bool PostPerform()
        {
            GWorld.Instance.GetQueue(PlaceOfInterestType.researchArea).AddResourse(target);
            GWorld.Instance.GetWorld().ModifyState(States.freeResearchArea, 1);
            return true;
        }       
    }
}

