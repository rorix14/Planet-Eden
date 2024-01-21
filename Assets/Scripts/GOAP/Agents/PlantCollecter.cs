namespace RPG.GOAP
{
    public class PlantCollecter : GAgent
    {
        private void Start()
        {
            SubGoal s1 = new SubGoal(States.deliverPlant, 1, false);
            goals.Add(s1, 2);

            SubGoal s2 = new SubGoal(States.Waiting, 1, false);
            goals.Add(s2, 3);

            if (!GWorld.Instance.GetQueue(PlaceOfInterestType.CollectablePlants).HasResource()) beliefs.ModifyState(States.Wait, 0);
        }
    }
}