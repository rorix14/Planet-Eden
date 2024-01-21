using UnityEngine;
using System;
using UnityEngine.Events;
using RPG.Utils;
using RPG.Stats;
using RPG.Core;
using RPG.Saving;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private GameObject healEffectPrefab = null;
        [Range(0, 100)]
        [SerializeField] private float regenarationPercentage = 70f;
        public TakeDamageEvent takeDamage;
        [Serializable] public class TakeDamageEvent : UnityEvent<float> { }
        [SerializeField] private UnityEvent HealthRestore;
        private LazyValue<float> healthPoints;
        private BaseStats baseStats = null;
        private ParticleSystem healEffect = null;
        public event Action OnHit, OnDeath, OnHealthChanged;

        public bool IsDead { get; private set; }
        public float HealthPoints => healthPoints.value;
        public float MaxHealthPoints => baseStats.GetStat(Stat.Health);
        public float HealthPercentage => 100 * healthPoints.value / baseStats.GetStat(Stat.Health);

        private void Awake()
        {
            baseStats = GetComponent<BaseStats>();
            healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth() => baseStats.GetStat(Stat.Health);
       
        private void Start() => healthPoints.ForceInit();

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);
           // print(gameObject.name + " took damage " + damage + " and current health is " + healthPoints.value);

            if (healthPoints.value == 0)
            {
                OnDeath?.Invoke();
                AwardExperience(instigator);
                Die();
            }
            else
            {
                OnHit?.Invoke();
                takeDamage?.Invoke(damage);
            }

            OnHealthChanged?.Invoke();
        }

        public void Heal(float healing)
        {
            healthPoints.value = Mathf.Min(MaxHealthPoints, healthPoints.value + healing);
            HealthRestore?.Invoke();
            OnHealthChanged?.Invoke();
        }

        public void PlayHealEffect()
        {
            if (!healEffect)
            {
                healEffect = Instantiate(healEffectPrefab, transform).GetComponentInChildren<ParticleSystem>();
                healEffect.Play();
            }
            else healEffect.Play();
        }

        private void RegenerateHealth()
        {
            float reagenHealthPoints = baseStats.GetStat(Stat.Health) * (regenarationPercentage / 100);
            healthPoints.value = Mathf.Max(healthPoints.value, reagenHealthPoints);
            OnHealthChanged?.Invoke();
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience) experience.GainExperience(baseStats.GetStat(Stat.ExperienceReward));
        }

        private void Die()
        {
            if (IsDead) return;
            IsDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public object CaptureState() => healthPoints.value;

        public void RestoreState(object state)
        {
            healthPoints.value = (float)state;

            if (healthPoints.value <= 0) Die();
        }

        private void OnEnable() => baseStats.onLevelUpEvent += RegenerateHealth;
        private void OnDisable() => baseStats.onLevelUpEvent -= RegenerateHealth;
    }
}
