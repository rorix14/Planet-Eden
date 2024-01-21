using UnityEngine;

namespace RPG.GOAP
{
    public class MakePublesPudles : MonoBehaviour
    {
        [SerializeField] private States state;
        [SerializeField] private float stateStrength;
        [SerializeField] private float stateDecayRate;
        [SerializeField] private GameObject resoursePrefab;
        [SerializeField] private PlaceOfInterestType queueName;
        [SerializeField] private States worldState;
        [SerializeField] private GAction action;
        private WorldStates beliefs;
        private bool stateFound = false;
        private float intialStenght;

        private void Awake()
        {
            beliefs = GetComponent<GAgent>().Beliefs;
            intialStenght = stateStrength;
        }

        private void LateUpdate()
        {
            if (action.Runnning)
            {
                stateFound = false;
                stateStrength = intialStenght;
            }

            if (!action.Runnning && beliefs.HasState(state)) stateFound = true;

            if (stateFound)
            {
                stateStrength -= stateDecayRate * Time.deltaTime;
                if (stateStrength <= 0)
                {
                    // Make a puddle and add it to a new que
                    Vector3 location = new Vector3(transform.position.x, resoursePrefab.transform.position.y, transform.position.z);
                    GameObject resourse = Instantiate(resoursePrefab, location, resoursePrefab.transform.rotation);
                    stateFound = false;
                    stateStrength = intialStenght;
                    beliefs.RemoveState(state);
                    GWorld.Instance.GetQueue(queueName).AddResourse(resourse);
                    GWorld.Instance.GetWorld().ModifyState(worldState, 1);
                }
            }
        }
    }
}