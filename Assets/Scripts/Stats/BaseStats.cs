using RPG.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass = CharacterClass.Grunt;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpEffectPrefab = null;
        [SerializeField] bool shouldUseModifier = false;
        [SerializeField] UnityEvent OnLevelUp = null;
        LazyValue<int> currentLevel;
        public event Action onLevelUpEvent;
        Experience experience;
        ParticleSystem levelUpEffect = null;

        public int Level => currentLevel.value;
        public Progression Progression => progression;

        private void Awake()
        {
            experience = GetComponent<Experience>();
            currentLevel = new LazyValue<int>(CalculateLevel);
        }

        private void Start() => currentLevel.ForceInit();

        public float GetStat(Stat stat) => (progression.GetStat(stat, characterClass, Level) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat) / 100);

        private float GetAdditiveModifier(Stat stat)
        {
            if (!shouldUseModifier) return 0f;

            float total = 0;
            foreach (IModdifierProvider provider in GetComponents<IModdifierProvider>())
                AcumulateModifiers(ref total, provider.GetAdditiveModifiers, stat);

            return total;
        }

        private float GetPercentageModifier(Stat stat)
        {
            if (!shouldUseModifier) return 0f;

            float total = 0;
            foreach (IModdifierProvider provider in GetComponents<IModdifierProvider>())
                AcumulateModifiers(ref total, provider.GetPercentageModifiers, stat);

            return total;
        }

        private void AcumulateModifiers(ref float total, Func<Stat, IEnumerable<float>> func, Stat stat)
        {
            foreach (float modifiers in func(stat)) total += modifiers;
        }

        private int CalculateLevel()
        {
            experience = GetComponent<Experience>();
            if (!experience) return startingLevel;

            float currentXP = experience.ExperiencePoints;

            for (int i = 1; i <= progression.GetLevels(Stat.ExperienceToLevelUp, characterClass); i++)
            {
                if (progression.GetStat(Stat.ExperienceToLevelUp, characterClass, i) > currentXP) return i;
            }

            return progression.GetLevels(Stat.ExperienceToLevelUp, characterClass) + 1;
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();

            if (newLevel > currentLevel.value)
            {
                if (!levelUpEffect)
                {
                    levelUpEffect = Instantiate(levelUpEffectPrefab, transform).GetComponentInChildren<ParticleSystem>();
                    levelUpEffect.Play();
                }
                else levelUpEffect.Play();

                currentLevel.value = newLevel;
                OnLevelUp?.Invoke();
                onLevelUpEvent?.Invoke();
            }
        }

        private void OnEnable()
        {
            if (experience) experience.onExperienceGained += UpdateLevel;
        }

        private void OnDisable()
        {
            if (experience) experience.onExperienceGained -= UpdateLevel;
        }
    }
}
