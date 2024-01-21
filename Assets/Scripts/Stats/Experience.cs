using UnityEngine;
using RPG.Saving;
using System;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0f;
        public event Action onExperienceGained;
        public event Action onExperienceUpdated;
        public float ExperiencePoints => experiencePoints;

        public void GainExperience(float experience)
        {
            experiencePoints += experience;
            onExperienceGained?.Invoke();
            onExperienceUpdated?.Invoke();
        }

        public object CaptureState() => experiencePoints;

        public void RestoreState(object state) => experiencePoints = (float)state;
    }
}
