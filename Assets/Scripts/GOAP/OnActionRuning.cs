using UnityEngine;

namespace RPG.GOAP
{
    public class OnActionRuning : GStateMonitor
    {
        private void Start() => stateStrength = 0;

        private void Update()
        {
            if (action.Runnning)
            {
                stateStrength += Random.Range(0f, stateChangeRate) * Time.deltaTime;
                if (stateStrength >= intialStenght)
                {
                   StartCoroutine(Sayline());
                    stateFound = false;
                    stateStrength = 0;
                }
            }
            else stateStrength = 0;
        }
    }
}