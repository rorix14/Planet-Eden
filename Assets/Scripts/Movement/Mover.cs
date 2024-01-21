using RPG.Core;
using RPG.Attributes;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] private float maxSpeed = 6f;
        [SerializeField] private float maxNavPathLength = 40f;
        private NavMeshAgent navMeshAgent = null;
        private Health health = null;
        private ActionScheduler actionScheduler = null;
        private Animator animator = null;
        private CapsuleCollider capsuleCollider = null;

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
            capsuleCollider = GetComponent<CapsuleCollider>();
        }

        private void Update()
        {
            navMeshAgent.enabled = !health.IsDead;
            capsuleCollider.enabled = !health.IsDead;
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            actionScheduler.StarAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destnation, float speedFraction)
        {
            navMeshAgent.destination = destnation;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }

        public bool TargetIsAtDestination()
        {
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance &&
                (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0)) return true;

            return false;
        }

        public bool CanMoveTo(Vector3 destination, float maxDistanceTraveled = 0)
        {
            NavMeshPath path = new NavMeshPath();
            if (!NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path)) return false;

            if (path.status != NavMeshPathStatus.PathComplete) return false;

            maxDistanceTraveled = maxDistanceTraveled == 0 ? maxNavPathLength : maxDistanceTraveled;

            if (GetpathLength(path) > maxDistanceTraveled) return false;

            return true;
        }

        private float GetpathLength(NavMeshPath path)
        {
            float total = 0;
            if (path.corners.Length < 2) return total;

            for (int i = 0; i < path.corners.Length - 1; i++) total += Vector3.Distance(path.corners[i], path.corners[i + 1]);

            return total;
        }

        public void Cancel() => navMeshAgent.isStopped = true;

        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            animator.SetFloat("forwardSpeed", localVelocity.z);
        }

        [System.Serializable]
        struct MoverData
        {
            public SerializableVector3 position;
            public SerializableVector3 rotation;
        }

        public object CaptureState()
        {
            MoverData data = new MoverData
            {
                position = new SerializableVector3(transform.position),
                rotation = new SerializableVector3(transform.eulerAngles)
            };
            return data;
        }

        public void RestoreState(object state)
        {
            MoverData data = (MoverData)state;
            navMeshAgent.Warp(data.position.ToVector());
            transform.eulerAngles = data.rotation.ToVector();
            actionScheduler.CancelCurrentAction();
        }
    }
}
