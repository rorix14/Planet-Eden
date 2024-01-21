using System.Collections;
using UnityEngine;

namespace RPG.GOAP
{
    public class ChuruFarmar : GAgent
    {
        private void Start()
        {
            SubGoal s1 = new SubGoal(States.waterPlant, 1, false);
            goals.Add(s1, 1);

            SubGoal s2 = new SubGoal(States.Dance, 1, false);
            goals.Add(s2, 2);

            StartCoroutine(GoDance());
        }

        private IEnumerator GoDance()
        {
            yield return new WaitForSeconds(Random.Range(20, 40));
            beliefs.ModifyState(States.mustDance, 0);
            StartCoroutine(GoDance());
        }
    }
}