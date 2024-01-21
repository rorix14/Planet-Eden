using UnityEngine;
using UnityEngine.AI;
using RPG.Movement;

namespace RPG.Control
{
    [System.Serializable]
    public class SearchBehavior
    {
        [SerializeField] private float radius = 0.8f;
        [SerializeField] private int findWaypointAttenpts = 50;
        private Vector3 target;
        private Mover mover = null;
        private Transform agent = null;

        public Mover Mover { set => mover = value; }
        public Transform Agent { set => agent = value; }
        public float Radius => radius;
        public Vector3 Target => target; 

        public Vector3 FindSearchPoint()
        {
            Vector3 target = agent.position;

            for (int i = 0; i < findWaypointAttenpts; i++)
            {
                if (!HasPositionInNavMesh(ref target)) continue;

                if (!mover.CanMoveTo(target, radius * 2)) continue;

                break;
            }

            this.target = target;
            return target;
        }

        private bool HasPositionInNavMesh(ref Vector3 target)
        {
            Vector3 sphereCenter = agent.position + agent.forward * radius;
            Vector3 randomPoint = sphereCenter + Random.insideUnitSphere * radius;

            NavMeshHit navMeshHit;
            if (!NavMesh.SamplePosition(randomPoint, out navMeshHit, 1, NavMesh.AllAreas)) return false;

            target = navMeshHit.position;
            return true;
        }
    }
}