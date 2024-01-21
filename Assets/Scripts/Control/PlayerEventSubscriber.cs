using UnityEngine;
using RPG.Inventory;
using RPG.Attributes;
using RPG.SFX;

namespace RPG.Control
{
    public class PlayerEventSubscriber : MonoBehaviour
    {
        [SerializeField] private SFXDB SFX = null;
        [SerializeField] private AudioSource onHit, onDeath;
        private InventoryWeaponsUI inventoryUI = null;
        private PlayerController playerController = null;
        private Health health;

        private void Awake()
        {
            inventoryUI = FindObjectOfType<InventoryWeaponsUI>();
            playerController = GetComponent<PlayerController>();
            health = GetComponent<Health>();
        }

        public void PlayRamdomHurtSound()
        {
            onHit.clip = SFX.HurtAudioClips[Random.Range(0, SFX.HurtAudioClips.Length)];
            onHit.Play();
        }

        public void PlayRamdomDeathSound()
        {
            onDeath.clip = SFX.DeadAudioClips[Random.Range(0, SFX.DeadAudioClips.Length)];
            onDeath.Play();
        }

        public void RemovePlayerControl(bool remove) => playerController.enabled = remove;

        private void OnEnable()
        {
            inventoryUI.RemovePlayerControl += RemovePlayerControl;
            health.OnHit += PlayRamdomHurtSound;
            health.OnDeath += PlayRamdomDeathSound;
        }

        private void OnDisable()
        {
            inventoryUI.RemovePlayerControl -= RemovePlayerControl;
            health.OnHit -= PlayRamdomHurtSound;
            health.OnDeath -= PlayRamdomDeathSound;
        }
    }
}