using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using RPG.Core;
using RPG.Attributes;
using RPG.Utils;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] ItemPool itemClass;
        [SerializeField] float projectileSpeed = 1f;
        [SerializeField] float lifeAfterInpact = 3f;
        [SerializeField] GameObject hitEffectPrefab = null;
        [SerializeField] bool isHoming = false;
        [SerializeField] float homingTurnAbility = 0.04f;
        [SerializeField] GameObject[] disableOnHit = null;
        [SerializeField] UnityEvent OnImpactEvent;
        Health target = null;
        GameObject instigator = null;
        float damage = 0;
        ConsumablePool itemPool;
        float initialSpeed;
        private ParticleSystem inpactEffect = null;
        private AudioSource audioSource;

        public ItemPool ItemClass => itemClass;

        private void Awake()
        {
            audioSource = GetComponentInChildren<AudioSource>();
            itemPool = FindObjectOfType<ConsumablePool>();
            initialSpeed = projectileSpeed;
        }

        private void OnEnable()
        {
            projectileSpeed = initialSpeed;
            GetComponent<BoxCollider>().enabled = true;
            foreach (GameObject toDisable in disableOnHit) toDisable.SetActive(true);
        }

        private void LateUpdate()
        {
            if (!target) return;
            if (isHoming && !target.IsDead)
            {
                Vector3 dir = Vector3.Normalize(AimLocation - transform.position);
                Quaternion rot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, homingTurnAbility * Time.deltaTime);
            }

            transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;
            transform.LookAt(AimLocation);
        }

        private Vector3 AimLocation => !target.GetComponent<CapsuleCollider>() ?
            target.transform.position : target.transform.position + (Vector3.up * target.GetComponent<CapsuleCollider>().height / 2);

        private void OnTriggerEnter(Collider other)
        {
            Health collisionHealth = other.GetComponent<Health>();
            if (!collisionHealth)
            {
                ProcessHit(other.transform);
                return;
            }

            if (collisionHealth != target) return;

            if (target.IsDead) return;

            target.TakeDamage(instigator, damage);
            ProcessHit(target.transform);
        }

        void ProcessHit(Transform hitPlace)
        {
            projectileSpeed = 0;

            // create sepecific item on charater model and place it insted of hits Hips
            Transform hitCharacter = hitPlace.FindChildByRecursion("Hips");
            transform.parent = hitCharacter ? hitCharacter : hitPlace;
         
            if (hitEffectPrefab)
            {
                if (!inpactEffect)
                {
                    inpactEffect = Instantiate(hitEffectPrefab, transform.position, transform.rotation).GetComponent<ParticleSystem>();
                    //inpactEffect.gameObject.transform.SetParent(transform);
                    inpactEffect.Play();
                }
                else
                {
                    inpactEffect.transform.position = transform.position;
                    inpactEffect.transform.rotation = transform.rotation;
                    inpactEffect.Play();
                }
            }

            GetComponent<BoxCollider>().enabled = false;
            OnImpactEvent?.Invoke();

            foreach (GameObject toDisable in disableOnHit) toDisable.SetActive(false);

            StartCoroutine(DestroyProjectile());
        }

        private IEnumerator DestroyProjectile()
        {
            yield return new WaitForSeconds(Mathf.Max(lifeAfterInpact, audioSource.clip.length));
            itemPool.ReturnToPool(itemClass, gameObject);
        }

        //TO HIT EVERY TARGET, but must make some modifications
        //private void OnTriggerEnter(Collider other)
        //{
        //    Health hitHealth = other.GetComponent<Health>();

        //    if (hitHealth == null) Destroy(gameObject);

        //    if (hitHealth != null && !hitHealth.IsDead())
        //    {
        //        hitHealth.TakeDamage(damage);
        //        Destroy(gameObject);
        //    }
        //}
    }
}

