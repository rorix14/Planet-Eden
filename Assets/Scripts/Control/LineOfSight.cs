using UnityEngine;
using UnityEngine.AI;
using RPG.Utils;

namespace RPG.Control
{
    [System.Serializable]
    public class LineOfSight
    {
        [SerializeField] private float viewRadius;
        [Range(0, 360)]
        [SerializeField] private float viewAngle;
        [SerializeField] private float minDetectionDistance;
        [Range(0, 360)]
        [SerializeField] private float minDetectionViewAngle;
        private Transform target = null;
        private Transform agent = null;
        private Vector3 agentEyeSight;

        public Transform Agent { set => agent = value; }
        public Vector3 AgentEyeSight { set => agentEyeSight = value; }
        public Transform Target => target;
        public float MinDetectionDistance => minDetectionDistance;
        public float MinDetectionViewAngle => minDetectionViewAngle;
        public float ViewRadius => viewRadius;
        public float ViewAngle => viewAngle;

        public bool CanSeeTarget(Transform target)
        {
            this.target = null;

            if (Vector3.Distance(target.position, agent.position) > viewRadius) return false;

            return TargetInLineOfSight(target, viewRadius, viewAngle)
                ? true : TargetInLineOfSight(target, minDetectionDistance, minDetectionViewAngle);
        }

        private bool TargetInLineOfSight(Transform target, float viewRadius, float viewAngle)
        {
            RaycastHit[] targetsInViewRadius = Physics.SphereCastAll(agent.position, viewRadius, Vector3.up, 0f);

            for (int i = 0; i < targetsInViewRadius.Length; i++)
            {
                Transform objInView = targetsInViewRadius[i].transform;

                if (objInView != target) continue;
                Vector3 targetEyeSight = new Vector3(0, target.GetComponent<NavMeshAgent>().height * 0.9f, 0);

                Vector3 dirTotarget = ((objInView.position + targetEyeSight) - (agent.position + agentEyeSight)).normalized;

                if (Vector3.Angle(agent.forward, dirTotarget) < viewAngle / 2)
                {
                    float disToTarget = Vector3.Distance(agent.position + agentEyeSight, objInView.position + targetEyeSight);

                    RaycastHit[] rayToPlayer = Physics.RaycastAll(agent.position + agentEyeSight, dirTotarget, disToTarget);

                    foreach (RaycastHit ray in rayToPlayer.SortByDistance())
                    {
                        if (ray.transform != target && ray.transform != agent) return false;

                        if (ray.transform == target)
                        {
                            this.target = ray.collider.transform;
                            return true;
                        }
                    }
                }
            }

            return false;
        }     
    }
}