using UnityEngine;
using UnityEngine.AI;
using RPG.Movement;

namespace RPG.Control
{
    public class EnemySensors : MonoBehaviour
    {
        [SerializeField] private LineOfSight lineOfSight = null;
        [SerializeField] private SearchBehavior searchBehavior = null;
        private NavMeshAgent meshAgent = null;
        public LineOfSight LineOfSight => lineOfSight;
        public SearchBehavior SearchBehavior => searchBehavior;

        private void Awake()
        {
            meshAgent = GetComponent<NavMeshAgent>();
            lineOfSight.Agent = transform;
            lineOfSight.AgentEyeSight = new Vector3(0, meshAgent.height * 0.9f, 0);
            searchBehavior.Agent = transform;
            searchBehavior.Mover = GetComponent<Mover>();
        }

        private Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal) angleInDegrees += transform.eulerAngles.y;

            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
        
        private void OnDrawGizmosSelected()
        {
            // draw search behaviour
            Vector3 center = transform.position + (transform.forward * searchBehavior.Radius);
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(center, searchBehavior.Radius);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, searchBehavior.Target);

            // draw line of sight
            Gizmos.DrawWireSphere(transform.position, lineOfSight.MinDetectionDistance);
            Vector3 viewAngleC = DirFromAngle(-lineOfSight.MinDetectionViewAngle / 2, false);
            Vector3 viewAngleD = DirFromAngle(lineOfSight.MinDetectionViewAngle / 2, false);
            Gizmos.DrawLine(transform.position, transform.position + viewAngleC * lineOfSight.MinDetectionDistance);
            Gizmos.DrawLine(transform.position, transform.position + viewAngleD * lineOfSight.MinDetectionDistance);

            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, lineOfSight.ViewRadius);
            Vector3 viewAngleA = DirFromAngle(-lineOfSight.ViewAngle / 2, false);
            Vector3 viewAngleB = DirFromAngle(lineOfSight.ViewAngle / 2, false);
            Gizmos.DrawLine(transform.position, transform.position + viewAngleA * lineOfSight.ViewRadius);
            Gizmos.DrawLine(transform.position, transform.position + viewAngleB * lineOfSight.ViewRadius);

            if (lineOfSight.Target != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position + new Vector3(0, meshAgent.height * 0.8f, 0), lineOfSight.Target.position + new Vector3(0, lineOfSight.Target.GetComponent<NavMeshAgent>().height, 0 ));
            }
        }
    }
}