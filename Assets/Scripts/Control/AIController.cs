using System.Collections.Generic;
using UnityEngine;
using RPG.Utils;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Attributes;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float shoutDistance = 2f;
        [SerializeField] float suspicionTime = 10f;
        [SerializeField] PatrolPath patrolPath = null;
        [SerializeField] float waypointTolurence = 1f;
        [SerializeField] float waypointDwellTime = 3f;
        [SerializeField] float searchPointDwellTime = 2f;
        [SerializeField] float aggroCooldownTime = 3f;
        [Range(0, 1)]
        [SerializeField] float patrolSpeedFraction = 0.5f;
        Fighter fighter;
        Health health;
        GameObject player;
        Mover mover;
        ActionScheduler actionScheduler;
        LazyValue<Vector3> guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArriveAtWaypoints = Mathf.Infinity;
        float timeSinceAgraveted = Mathf.Infinity;
        int currentWaypointIndex = 0;
        List<AIController> enemysAggroed = new List<AIController>();
        private EnemySensors enemySensors = null;
        private Vector3? currentSearchPoint = null;
        private bool persue;

        private void Awake()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
            enemySensors = GetComponent<EnemySensors>();
            guardPosition = new LazyValue<Vector3>(GetGuardPosition);
            player = GameObject.FindWithTag("Player");
        }

        private Vector3 GetGuardPosition() => transform.position;

        private void Start() => guardPosition.ForceInit();

        private void Update()
        {
            if (health.IsDead) return;

            if (fighter.CanAttack(player) && IsAgravated())
            {
                AttackBehaviour();
            }
            else if (fighter.CanAttack(player) && timeSinceLastSawPlayer < suspicionTime && !IsAgravated())
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }

            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArriveAtWaypoints += Time.deltaTime;
            timeSinceAgraveted += Time.deltaTime;
        }

        public void Aggravate() => timeSinceAgraveted = 0;

        private bool IsAgravated()
        {
            bool isAgravated = false;
            if (enemySensors.LineOfSight.CanSeeTarget(player.transform))
            {
                Aggravate();
                persue = true;
                isAgravated = true;
            }
            else if (timeSinceAgraveted < aggroCooldownTime)
            {
                persue = false;
                isAgravated = true;
            }

            return isAgravated;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition.value;

            if (patrolPath)
            {
                if (AtWayPoint())
                {
                    timeSinceArriveAtWaypoints = 0;
                    currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
                }

                nextPosition = CurrentWaypoint;
            }

            if (timeSinceArriveAtWaypoints > waypointDwellTime) mover.StartMoveAction(nextPosition, patrolSpeedFraction);
        }

        private bool AtWayPoint() => Vector3.Distance(transform.position, CurrentWaypoint) < waypointTolurence;

        private Vector3 CurrentWaypoint => patrolPath.GetWaypoint(currentWaypointIndex);

        private void SuspicionBehaviour()
        {
            enemysAggroed = new List<AIController>();
            actionScheduler.CancelCurrentAction();

            currentSearchPoint = currentSearchPoint ?? enemySensors.SearchBehavior.FindSearchPoint();

            if (mover.TargetIsAtDestination())
            {
                if (timeSinceArriveAtWaypoints > searchPointDwellTime) currentSearchPoint = enemySensors.SearchBehavior.FindSearchPoint();
            }
            else timeSinceArriveAtWaypoints = 0;

            mover.MoveTo(currentSearchPoint.Value, patrolSpeedFraction);
        }

        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            currentSearchPoint = null;
            fighter.Attack(player, persue);
            AggrevateNearbyEnemies();
        }

        private void AggrevateNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);

            foreach (RaycastHit enemy in hits)
            {
                AIController aIController = enemy.collider.GetComponent<AIController>();
                if (!aIController || aIController == this || enemysAggroed.Contains(aIController)) continue;

                enemysAggroed.Add(aIController);

                aIController.Aggravate();
            }
        }
    }
}