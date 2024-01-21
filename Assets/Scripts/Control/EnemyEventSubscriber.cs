using UnityEngine;
using RPG.Attributes;
using RPG.SFX;

namespace RPG.Control
{
    public class EnemyEventSubscriber : MonoBehaviour
    {
        [SerializeField] private SFXDB SFX = null;
        [SerializeField] private AudioSource onHit, onDeath;
        private Health health;

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        public void PlayRamdomHurtSound()
        {
            onHit.clip = SFX.AlienHurtAudioClips[Random.Range(0, SFX.AlienHurtAudioClips.Length)];
            onHit.Play();
        }

        public void PlayRamdomDeathSound()
        {
            onDeath.clip = SFX.AlienDeadAudioClips[Random.Range(0, SFX.AlienDeadAudioClips.Length)];
            onDeath.Play();
        }
        private void OnEnable()
        {
            health.OnHit += PlayRamdomHurtSound;
            health.OnDeath += PlayRamdomDeathSound;
        }

        private void OnDisable()
        {
            health.OnHit -= PlayRamdomHurtSound;
            health.OnDeath -= PlayRamdomDeathSound;
        }
    }
}

