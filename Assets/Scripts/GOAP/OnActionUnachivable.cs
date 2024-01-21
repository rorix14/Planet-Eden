using UnityEngine;

namespace RPG.GOAP
{
    public class OnActionUnachivable : GStateMonitor
    {
        private void Update()
        {
            if (action.Runnning)
            {
                stateFound = false;
                stateStrength = intialStenght;
            }

            if (!action.Runnning && beliefs.HasState(state)) stateFound = true;

            if (stateFound)
            {
                stateStrength -= stateChangeRate * Time.deltaTime;
                if (stateStrength <= 0)
                {
                    StartCoroutine(Sayline());
                    stateFound = false;
                    stateStrength = intialStenght;
                }
            }
        }
    }
}