using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using RPG.Control;
using RPG.Attributes;

namespace RPG.GOAP
{
    public class Churu : GAgent
    {
        [SerializeField] private float minEnemyDistance = 5f;
        [SerializeField] private float minPlayerDistance = 5f;
        [SerializeField] private float rotateToPlayerSpeed = 1.5f;
        private List<Health> enemiesHealth = null;
        private Terrified terrifiedAction = null;
        private GreetPlayer greetPlayer = null;
        private Transform player = null;
        private bool hasGreatedPlayer = false;

        protected override void Awake()
        {
            base.Awake();
            enemiesHealth = new List<Health>();
            CombatTarget[] enemies = FindObjectsOfType<CombatTarget>();
            foreach (CombatTarget enemie in enemies) enemiesHealth.Add(enemie.GetComponent<Health>());
            terrifiedAction = GetComponent<Terrified>();
            greetPlayer = GetComponent<GreetPlayer>();
            player = GameObject.FindWithTag("Player").transform;
        }

        private void Start()
        {
            SubGoal s1 = new SubGoal(States.Scavenge, 1, false);
            goals.Add(s1, 1);

            SubGoal s2 = new SubGoal(States.rested, 1, false);
            goals.Add(s2, 2);

            SubGoal s3 = new SubGoal(States.GreatPlayer, 1, true);
            goals.Add(s3, 3);

            SubGoal s4 = new SubGoal(States.Terrified, 1, false);
            goals.Add(s4, 4);

            StartCoroutine(GetTiered());
        }

        private void Update()
        {
            bool enemyIsInRange = false;
            foreach (Health enemy in enemiesHealth)
            {
                if (!enemy.IsDead && Vector3.Distance(transform.position, enemy.transform.position) < minEnemyDistance)
                {
                    enemyIsInRange = true;
                    if (!beliefs.HasState(States.isTerrified))
                    {
                        if (CurrentAction != null && terrifiedAction != null && CurrentAction != terrifiedAction) GAnimation.CancelCurrentAction(this, CurrentAction);

                        beliefs.ModifyState(States.isTerrified, 0);
                        break;
                    }
                }
            }

            if (!enemyIsInRange) beliefs.RemoveState(States.isTerrified);

            if (!hasGreatedPlayer && !beliefs.HasState(States.isTerrified) && Vector3.Distance(player.position, transform.position) < minPlayerDistance)
            {
                if (CurrentAction != null && greetPlayer != null && CurrentAction != greetPlayer) GAnimation.CancelCurrentAction(this, CurrentAction);

                beliefs.ModifyState(States.spotedPlayer, 0);
                hasGreatedPlayer = true;
            }

            if(greetPlayer?.Runnning == true)
            {
                Vector3 dir = Vector3.Normalize(player.transform.position - transform.position);
                Quaternion rot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotateToPlayerSpeed * Time.deltaTime);
            }
        }

        private IEnumerator GetTiered()
        {
            yield return new WaitForSeconds(Random.Range(30, 50));

            if (!beliefs.HasState(States.isTerrified)) beliefs.ModifyState(States.exhausted, 0);
            StartCoroutine(GetTiered());
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, minEnemyDistance);
        }
    }
}