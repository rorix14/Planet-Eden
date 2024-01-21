using System.Collections.Generic;
using UnityEngine;

namespace RPG.GOAP
{
    public class SubGoal
    {
        public Dictionary<States, int> sGoals;
        public bool remove;

        public SubGoal(States key, int val, bool remove)
        {
            sGoals = new Dictionary<States, int>();
            sGoals.Add(key, val);
            this.remove = remove;
        }
    }

    public abstract class GAgent : MonoBehaviour
    {
        [SerializeField] private GameObject dialogConteiner = null;
        public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();
        protected WorldStates beliefs = new WorldStates();
        protected GInventory inventory = new GInventory();
        public List<GAction> actions;
        private GPlanner planner;
        private Queue<GAction> actionQueue;
        private GAction currentAction;
        private SubGoal currentGoal;
        private bool invoked;
        private Vector3 destnation = Vector3.zero;
        private GAnimationManager gAnimation = null;

        public WorldStates Beliefs => beliefs;
        public GInventory Inventory => inventory;
        public GAction CurrentAction => currentAction;
        public GAnimationManager GAnimation => gAnimation;
        public GameObject DialogConteiner => dialogConteiner;

        protected virtual void Awake()
        {
            actions = new List<GAction>(GetComponents<GAction>());
            gAnimation = new GAnimationManager(gameObject);
        }

        private void EndOfAction() => invoked = false;

        private void LateUpdate()
        {
            gAnimation.UpdateAnimator(currentAction);

            if (currentAction && currentAction.Runnning)
            {
                float distanceToTarget = Vector3.Distance(destnation, transform.position);
                if (distanceToTarget < 1f)
                {
                    transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,
                        Mathf.LerpAngle(transform.localEulerAngles.y, currentAction.Target.transform.localEulerAngles.y, currentAction.RotateToTargetSpeed * Time.deltaTime),
                        transform.localEulerAngles.z);

                    if (!invoked)
                    {
                        gAnimation.CompleteAction(this, currentAction, EndOfAction);
                        invoked = true;
                    }
                }

                return;
            }

            if (planner == null || actionQueue == null)
            {
                planner = new GPlanner();

                List<KeyValuePair<SubGoal, int>> sortedGoals = new List<KeyValuePair<SubGoal, int>>(goals);
                sortedGoals.Sort((firstPair, nextPair) => firstPair.Value.CompareTo(nextPair.Value));
                sortedGoals.Reverse();

                foreach (KeyValuePair<SubGoal, int> goal in sortedGoals)
                {
                    actionQueue = planner.Plan(actions, goal.Key.sGoals, beliefs);

                    if (actionQueue != null)
                    {
                        currentGoal = goal.Key;
                        break;
                    }
                }
            }

            if (actionQueue != null && actionQueue.Count == 0)
            {
                if (currentGoal.remove) goals.Remove(currentGoal);

                planner = null;
            }

            if (actionQueue != null && actionQueue.Count > 0)
            {
                currentAction = actionQueue.Dequeue();

                if (currentAction.PrePerform())
                {
                    if (!currentAction.Target)
                        foreach (GPlaceOfInterest placeOfInterest in FindObjectsOfType<GPlaceOfInterest>())
                            if (placeOfInterest.ResorceType == currentAction.targetTag) currentAction.Target = placeOfInterest.gameObject;

                    if (currentAction.Target)
                    {
                        currentAction.Runnning = true;
                        destnation = currentAction.Target.transform.position;
                        StartCoroutine(gAnimation.MoveToGoal(destnation));
                    }
                }
                else actionQueue = null;
            }
        }
    }
}