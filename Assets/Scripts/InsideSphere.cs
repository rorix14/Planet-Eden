using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class InsideSphere : MonoBehaviour
{
    [SerializeField] private float radius = 0.8f;
    private Vector3 center;
    private Vector3 randomPoint;
    Vector3 target = Vector3.zero;
    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        StartCoroutine(FindPoint());
    }

    IEnumerator FindPoint()
    {
        if (!HasPositionInNavMesh(out Vector3 target)) target = transform.position;

        agent.SetDestination(target);
        this.target = target;
        yield return new WaitForSeconds(2f);

        StartCoroutine(FindPoint());
    }

    private bool HasPositionInNavMesh(out Vector3 target)
    {
        target = new Vector3();

        Vector3 sphereCenter = transform.position + transform.forward * radius;    
        //Vector3 randomPoint = center + UnityEngine.Random.insideUnitSphere * radius;

        Vector3 randomPoint = sphereCenter + RandomPointInUnitShere() * radius;

        NavMeshHit navMeshHit;
        if (!NavMesh.SamplePosition(randomPoint, out navMeshHit, 1, NavMesh.AllAreas)) return false;

        target = navMeshHit.position;
        return true;
    }

    private Vector3 RandomPointInUnitShere()
    {
        Vector3 randomPoint;

        do
        {
            randomPoint = new Vector3(UnityEngine.Random.Range(-1,1), UnityEngine.Random.Range(-1,1), UnityEngine.Random.Range(-1,1));
        }
        while(!Check(randomPoint.x, randomPoint.y, randomPoint.z, new Vector3(0,0,0)));

        return randomPoint;
    }

    public bool Check(float x, float y, float z, Vector3 center)
    {
        // remove center?
        //center is 0
        float x1 = (float)Math.Pow((x - center.x), 2);
        float y1 = (float)Math.Pow((y - center.y), 2);
        float z1 = (float)Math.Pow((z - center.z), 2);

        float ans = (x1 + y1 + z1);

        return Math.Sqrt (ans) <= radius;
    }

    private void OnDrawGizmos()
    {
        Vector3 center = transform.position + (transform.forward * radius);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(center, radius);
        Gizmos.color = Color.red;
        var test = new Vector3(transform.position.x + (transform.forward.x * radius), transform.position.y, transform.position.z + (transform.forward.z * radius));
        Gizmos.DrawIcon(target, "point", true);
        Gizmos.DrawLine(transform.position, target);
    }
}
