namespace RPG.GOAP
{
    public class Scavenger : GAgent
    {
        private void Start()
        {
            SubGoal s1 = new SubGoal(States.Scavenge, 1, false);
            goals.Add(s1, 1);
        }
    }
}
