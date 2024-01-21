using System.Collections;
using System;
using UnityEngine;
using UnityEngine.AI;

public class GAnimationManager
{
    private Animator animator = null;
    private NavMeshAgent navAgent = null;
    private GameObject agent;

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

    public IEnumerator CompleteAction(GAction currentAction, Action endOfAction)
    {
        float duration = 0f;
        float repeateAnimationTime = 0f;
        SetActionAnimation(currentAction);

        yield return new WaitUntil(TargetIsAtDestination);

       if(currentAction.AnimatorOverride)
        {
            animator.SetTrigger("attack");
            animator.ResetTrigger("stopAttack");
        } 

        yield return new WaitForEndOfFrame();

        while (duration <= currentAction.ActionDuration)
        {
            if (repeateAnimationTime >= animator.GetCurrentAnimatorStateInfo(0).length)
            {
                if (currentAction.AnimatorOverride) animator.SetTrigger("attack");
                repeateAnimationTime = 0;
            }

            repeateAnimationTime += Time.deltaTime;
            duration += Time.deltaTime;
            yield return null;
        }

        animator.SetTrigger("stopAttack");
        animator.ResetTrigger("attack");
        currentAction.Runnning = false;
        currentAction.PostPerform();
        endOfAction();
    }

    public bool TargetIsAtDestination()
    {
        if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance &&
            (!navAgent.hasPath || navAgent.velocity.sqrMagnitude == 0)) return true;

        return false;
    }

    private void SetActionAnimation(GAction currentAction)
    {
        AnimatorOverrideController overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
        if (currentAction.AnimatorOverride) animator.runtimeAnimatorController = currentAction.AnimatorOverride;
        //else if (overrideController) animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
    }
}