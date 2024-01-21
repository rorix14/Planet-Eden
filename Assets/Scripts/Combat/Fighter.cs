using System.Collections.Generic;
using UnityEngine;
using System;
using RPG.Attributes;
using RPG.Stats;
using RPG.Utils;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using UnityEngine.AI;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModdifierProvider
    {
        [SerializeField] Transform rigthHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] WeaponConfig defaultWeapon = null;
        Health target;
        Mover mover;
        ActionScheduler actionScheduler;
        Animator animator;
        float timeSinceLastAttack = Mathf.Infinity;
        WeaponConfig currentWeaponConfig = null;
        LazyValue<Weapon> currentWeapon = null;
        private bool persue = false;
        private BaseStats baseStats = null;
        public event Func<Projectile, bool> HasAmmo;
        public event Action OnCancelAttack = null;

        public Health Target => target;
        public WeaponConfig CurrentWeaponConfig => currentWeaponConfig;

        private void Awake()
        {
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
            baseStats = GetComponent<BaseStats>();
            currentWeaponConfig = defaultWeapon;
            currentWeapon = new LazyValue<Weapon>(() => AttachWeapon(defaultWeapon));
        }

        private void Start() => currentWeapon.ForceInit();

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (!target || target.IsDead) return;

            if (!GetIsInRange(target.transform))
            {
                if (persue)
                {
                    NavMeshAgent targetNavMeshAgent = target.GetComponent<NavMeshAgent>();
                    float prediction = (target.transform.position - transform.position).magnitude / targetNavMeshAgent.speed;

                    Vector3 predictedTargetPosition = target.transform.position + (targetNavMeshAgent.velocity * prediction);
                    mover.MoveTo(predictedTargetPosition, 1f);
                }
                else mover.MoveTo(target.transform.position, 1f);
            }
            else
            {
                mover.Cancel();
                AttackBehaviour();
            }
        }

        public void EquipWeapon(WeaponConfig weapon)
        {
            currentWeaponConfig = weapon;
            currentWeapon.value = AttachWeapon(weapon);
        }

        private bool GetIsInRange(Transform target) => Vector3.Distance(transform.position, target.transform.position) < currentWeaponConfig.WeaponRange;

        private Weapon AttachWeapon(WeaponConfig weapon) => weapon.Spawn(rigthHandTransform, leftHandTransform, animator);

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);

            if (timeSinceLastAttack > currentWeaponConfig.TimeBetweenAttacks)
            {
                // This will trigger the Hit() event.
                animator.ResetTrigger("stopAttack");
                animator.SetTrigger("attack");
                timeSinceLastAttack = 0f;
            }
        }

        // Animation Event.
        void Hit()
        {
            if (!target || !GetIsInRange(target.transform)) return;

            float damage = baseStats.GetStat(Stat.Damage);

            if (currentWeapon.value) currentWeapon.value.OnHit();

            if (currentWeaponConfig.HasProjectile)
            {
                // CHNAGE IF YOU DON'T WHANT FIERING ANIMATION TO TRIGGER
                if (HasAmmo != null)
                {
                    foreach (Func<Projectile, bool> func in HasAmmo.GetInvocationList())
                    {
                        if (func.Invoke(currentWeaponConfig.HasProjectile))
                        {
                            currentWeaponConfig.LaunchProjectile(rigthHandTransform, leftHandTransform, target, gameObject, damage);
                            break;
                        }
                    }
                }
                else currentWeaponConfig.LaunchProjectile(rigthHandTransform, leftHandTransform, target, gameObject, damage);
            }
            else target.TakeDamage(gameObject, damage);
        }

        // Animation Event.
        void Shoot() => Hit();

        public void Attack(GameObject combatTarget, bool canPersue)
        {
            target = combatTarget.GetComponent<Health>();
            persue = canPersue;
            actionScheduler.StarAction(this);
        }

        public void Cancel()
        {
            animator.SetTrigger("stopAttack");
            animator.ResetTrigger("attack");
            OnCancelAttack?.Invoke();
            target = null;
            mover.Cancel();
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage) yield return currentWeaponConfig.WeaponDamage;
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage) yield return currentWeaponConfig.DamagePercentageBonus;
        }

        public bool CanAttack(GameObject target) => !mover.CanMoveTo(target.transform.position) && !GetIsInRange(target.transform) ?
            false : target && !target.GetComponent<Health>().IsDead;

        public object CaptureState() => currentWeaponConfig.name;

        public void RestoreState(object state)
        {
            WeaponConfig weapon = Resources.Load<WeaponConfig>((string)state);
            EquipWeapon(weapon);
        }
    }
}