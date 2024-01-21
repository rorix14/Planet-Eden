using UnityEngine;

namespace RPG.GOAP
{
    public class ChuruFirstContact : GAgent
    {
        [SerializeField] private float minPlayerDistance = 5f;
        [SerializeField] private float rotateSpeed = 1.5f;
        private Transform player = null;
        private Vector3 initialAngle;
     
        protected override void Awake()
        {
            base.Awake();
            player = GameObject.FindWithTag("Player").transform;
            initialAngle = transform.localEulerAngles;
        }
        void Start()
        {
            SubGoal s1 = new SubGoal(States.GreatPlayer, 1, false);
            goals.Add(s1, 1);
        }

        void Update()
        {
            if (Vector3.Distance(player.position, transform.position) < minPlayerDistance)
            {
              if(!beliefs.HasState(States.spotedPlayer))  beliefs.ModifyState(States.spotedPlayer, 0);

                Vector3 dir = Vector3.Normalize(player.transform.position - transform.position);
                Quaternion rot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotateSpeed * Time.deltaTime);
            }
            else
            {
                beliefs.RemoveState(States.spotedPlayer);
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,
                  Mathf.LerpAngle(transform.localEulerAngles.y, initialAngle.y, rotateSpeed * Time.deltaTime),
                  transform.localEulerAngles.z);
            }
        }
    }
}
