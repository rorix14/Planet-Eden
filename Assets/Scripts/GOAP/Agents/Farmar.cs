using System.Collections;
using UnityEngine;

namespace RPG.GOAP
{
    public class Farmar : GAgent
    {
        private void Start()
        {
            SubGoal s1 = new SubGoal(States.waterPlant, 1, false);
            goals.Add(s1, 1);

            SubGoal s2 = new SubGoal(States.rested, 1, false);
            goals.Add(s2, 2);

            StartCoroutine(GetTiered());
        }

        private IEnumerator GetTiered()
        {
            yield return new WaitForSeconds(Random.Range(20, 40));
            beliefs.ModifyState(States.exhausted, 0);
            StartCoroutine(GetTiered());
        }
    }
}