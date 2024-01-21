using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] UnityEvent onHit;
        [SerializeField] private GameObject onHitEffectPrefab = null;
        private ParticleSystem onHitEffect = null;

        public void OnHit()
        {
            if (onHitEffectPrefab)
            {
                if (!onHitEffect)
                {
                    onHitEffect = Instantiate(onHitEffectPrefab, transform.position, transform.rotation).GetComponent<ParticleSystem>();
                    onHitEffect.Play();
                }
                else
                {
                    onHitEffect.transform.position = transform.position;
                    onHitEffect.transform.rotation = transform.rotation;
                    onHitEffect.Play();
                }
            }

            onHit?.Invoke();
        }
    }
}