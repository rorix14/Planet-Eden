using UnityEngine;

namespace RPG.Core
{
    // to remove
    public class DestroyAfterEffect : MonoBehaviour
    {
        [SerializeField] GameObject targetToDestroy = null;
        void Update()
        {
            if (!GetComponent<ParticleSystem>().IsAlive())
            {
                if (targetToDestroy) Destroy(targetToDestroy);
                else Destroy(gameObject);
            }
        }
    }
}
