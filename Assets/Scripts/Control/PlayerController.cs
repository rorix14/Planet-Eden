using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using RPG.Movement;
using RPG.Attributes;
using RPG.Utils;
using RPG.Core;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private GameObject moveEffectPrefab;
        [SerializeField] private AudioSource cursorActionAudio;
        private Health health;
        private Mover mover;
        private ActionScheduler actionScheduler;

        [Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
            public AudioClip audioClip;
        }
        [SerializeField] CursorMapping[] curserMappings = null;
        [SerializeField] float maxNavMeshProjectionDistance = 1f;
        [SerializeField] float raycastRadius = 0.2f;
        private ParticleSystem moveEffect = null;

        private void Awake()
        {
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
        }

        void Update()
        {
            if (IneractWithUI()) return;

            if (health.IsDead)
            {
                SetCursor(CursorType.None);
                return;
            }

            CancelCurrentAction();
            //PlayActionSound();
            if (InteractWithComponent()) return;
            if (InteractWithMovement()) return;
            SetCursor(CursorType.None);
        }
    
        private bool InteractWithComponent()
        {
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), raycastRadius);
            foreach (RaycastHit hit in hits.SortByDistance())
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();

                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IneractWithUI()
        {
            if (EventSystem.current.IsPointerOverGameObject()) SetCursor(CursorType.UI);
            return EventSystem.current.IsPointerOverGameObject();
        }

        private bool InteractWithMovement()
        {
            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);

            if (!hasHit) return false;

            if (!mover.CanMoveTo(target)) return false;

            if (Input.GetMouseButton(0))
            {
                ShowMoveEffect(target);
                mover.StartMoveAction(target, 1f);
            }

            SetCursor(CursorType.Movement);
            return true;
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

            if (!hasHit) return false;

            NavMeshHit navMeshHit;
            if (!NavMesh.SamplePosition(hit.point, out navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas)) return false;

            target = navMeshHit.position;
            return true;
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
            cursorActionAudio.clip = mapping.audioClip;
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            CursorMapping cursorMapping = new CursorMapping();

            foreach (CursorMapping cursor in curserMappings)
            {
                if (!cursor.type.Equals(type)) continue;

                cursorMapping = cursor;
            }

            return cursorMapping;
        }

        private static Ray GetMouseRay() => Camera.main.ScreenPointToRay(Input.mousePosition);

        private void ShowMoveEffect(Vector3 targetPos)
        {
            if (!moveEffect)
            {
                moveEffect = Instantiate(moveEffectPrefab, targetPos, Quaternion.identity).GetComponentInChildren<ParticleSystem>();
                moveEffect.Play();
            }
            else
            {
                if (moveEffect.isPlaying) moveEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

                moveEffect.transform.position = targetPos + new Vector3(0, 0.3f, 0);
                moveEffect.Play();
            }
        }

        private void PlayActionSound()
        {
            if (cursorActionAudio.clip == null) return;

            if (Input.GetMouseButtonDown(0)) cursorActionAudio.Play();
        }

        private void CancelCurrentAction()
        {
            if (Input.GetMouseButton(2)) actionScheduler.CancelCurrentAction();
        }
    }
}

//private bool InteractWithCombat()
//{
//    RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

//    foreach (RaycastHit hit in hits)
//    {
//        CombatTarget target = hit.transform.GetComponent<CombatTarget>();

//        if (!target) continue;

//        if (!fighter.CanAttack(target.gameObject)) continue;

//        if (Input.GetMouseButton(0)) fighter.Attack(target.gameObject);

//        SetCursor(CursorType.Combat);
//        return true;
//    }
//    return false;
//}
