using System.Collections;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.GOAP
{
    public class GAnimationManager
    {
        private Animator animator = null;
        private NavMeshAgent navAgent = null;
        private GameObject agent;
        private string animatorTrigger = "";
        private Coroutine currentActiveActionRotine = null;
        private float currentActiveActionDuration;
        private bool actionIsPerforming = false;

        public NavMeshAgent NavAgent => navAgent;

        public GAnimationManager(GameObject agent)
        {
            this.agent = agent;
            animator = agent.GetComponent<Animator>();
            navAgent = agent.GetComponent<NavMeshAgent>();
        }

        public void UpdateAnimator(GAction currentAction)
        {
            Vector3 velocity = navAgent.velocity;
            Vector3 localVelocity = agent.transform.InverseTransformDirection(velocity);
            navAgent.speed = currentAction ? currentAction.ActionVelocitty : navAgent.speed;
            animator.SetFloat("forwardSpeed", localVelocity.z);
        }
        
        // REFACTOR
        public void CancelCurrentAction(GAgent gAgent, GAction currentAction)
        {
           if (!actionIsPerforming)
            {
                currentAction.PostPerform();
                currentAction.Runnning = false;
                //Debug.Log("cancel action not running");
            }
            else
            {
                gAgent.StartCoroutine(MoveToGoal(gAgent.transform.position));
                currentActiveActionDuration = 0f;
                //Debug.Log("cancel action running");
            } 
        }

        // REFACTOR
        public void CompleteAction(GAgent gAgent,GAction currentAction, Action endOfAction)
        {
            currentActiveActionDuration = currentAction.ActionDuration;
            actionIsPerforming = true;
            currentActiveActionRotine = gAgent.StartCoroutine(CompleteActionRotine(currentAction, endOfAction));
        }

        private IEnumerator CompleteActionRotine(GAction currentAction, Action endOfAction)
        {
            System.Random rng = new System.Random();
            animatorTrigger = currentAction.AnimatorTrigger[rng.Next(0, currentAction.AnimatorTrigger.Length)];

            //Debug.Log("start action");
            yield return new WaitUntil(TargetIsAtDestination);

            animator.ResetTrigger("Walk");
            animator.SetTrigger(animatorTrigger);

            //yield return new WaitForSeconds(currentActiveActionDuration);
            //Debug.Log("is at destination");
            float timeRuning = 0f;
            while(timeRuning < currentActiveActionDuration)
            {
                timeRuning += Time.deltaTime;
                yield return null;
            }

            //Debug.Log("has completed");
            if (currentAction.PostPerform())
            {
                currentAction.Runnning = false;
                foreach (string trigger in currentAction.AnimatorTrigger) animator.ResetTrigger(trigger);
            }

            actionIsPerforming = false;
            endOfAction?.Invoke();
        }

        public IEnumerator MoveToGoal(Vector3 destination)
        {
            WaitForSeconds waitForSeconds = animatorTrigger == "Sit" ? new WaitForSeconds(2.2f) : null;
            animator.SetTrigger("Walk");

            //Debug.Log("move called");
            yield return waitForSeconds;

           // Debug.Log("destination set");
            navAgent.SetDestination(destination);
        }

        private bool TargetIsAtDestination()
        {
            if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance &&
                (!navAgent.hasPath || navAgent.velocity.sqrMagnitude == 0)) return true;

            return false;
        }
    }
}