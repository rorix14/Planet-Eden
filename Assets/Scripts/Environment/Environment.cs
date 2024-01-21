using UnityEngine;

namespace RPG.Environment
{
    public class Environment : MonoBehaviour
    {
        [SerializeField] private float minDistance;
        private Transform player = null;

        private void Awake() => player = GameObject.FindWithTag("Player").transform;

        void Update()
        {
            if (Vector3.Distance(player.transform.position, transform.position) < minDistance)
                foreach (Transform children in transform) children.gameObject.SetActive(true);
            else
                foreach (Transform children in transform) children.gameObject.SetActive(false);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, minDistance);
        }
    }
}
