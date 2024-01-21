namespace RPG.GOAP
{
    public class Doctor : GAgent
    {
        private void Start()
        {
            SubGoal s1 = new SubGoal(States.Research, 1, false);
            goals.Add(s1, 1);

            SubGoal s2 = new SubGoal(States.TreatPatient, 1, false);
            goals.Add(s2, 2);
        }
    }
}

