using System.Collections;
using UnityEngine;

namespace RPG.GOAP
{
    public class Patient : GAgent
    {
        [SerializeField] private GameObject bed = null;
        private Quaternion initilalRotaion;

        private void Start()
        {
            SubGoal s3 = new SubGoal(States.Sleep, 1, false);
            goals.Add(s3, 1);

            SubGoal s1 = new SubGoal(States.Cough, 1, false);
            goals.Add(s1, 2);

            SubGoal s2 = new SubGoal(States.GetTreated, 1, false);
            goals.Add(s2, 3);

            inventory.AddItem(bed);

            StartCoroutine(GetSick());
            StartCoroutine(StartCough());

            initilalRotaion = transform.rotation;
        }

        private IEnumerator GetSick()
        {
            yield return new WaitForSeconds(Random.Range(30, 50));
            beliefs.ModifyState(States.isSick, 0);
            StartCoroutine(GetSick());
        }

        private IEnumerator StartCough()
        {
            yield return new WaitForSeconds(Random.Range(10, 20));
            beliefs.ModifyState(States.needsToCough, 0);
            StartCoroutine(StartCough());
        }

        private void Update() => transform.rotation = initilalRotaion;
    }
}
