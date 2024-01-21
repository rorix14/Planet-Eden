using UnityEngine;
using RPG.Attributes;
using RPG.Combat;

namespace RPG.Control
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        [SerializeField] private Renderer[] bodyParts = null;
        private static CombatTarget playerChosenTarget;
        private Health health = null;
        private Fighter playerFighter = null;
        private bool isSubscribed = false;

        private void Awake() => health = GetComponent<Health>();

        public CursorType GetCursorType() => CursorType.Combat;

        public bool HandleRaycast(PlayerController callingController)
        {
            playerFighter = callingController.GetComponent<Fighter>();

            if (!playerFighter.CanAttack(gameObject)) return false;

            if (Input.GetMouseButton(0))
            {
                playerFighter.Attack(gameObject, false);
                SetTarget();
            }

            return true;
        }

        private void SetTarget()
        {
            if (playerChosenTarget != null)
            {
                SetOutline(playerChosenTarget, false);
            }

            playerChosenTarget = this;
            SetOutline(playerChosenTarget, true);
        }

        private void SetOutline(CombatTarget target, bool showOutline)
        {
            foreach (Renderer bodyPart in target.bodyParts) bodyPart.material.SetFloat("_Outline", showOutline ? 0.15f : 0f);

            Subscribe(target, showOutline);
        }

        private void DisabelOutline()
        {
            if (this != playerChosenTarget) return;

            SetOutline(this, false);
        }

        private void Subscribe(CombatTarget target, bool enabled)
        {
            if (playerFighter == null) return;

            if (!enabled) playerFighter.OnCancelAttack -= target.DisabelOutline;
            else if (!isSubscribed) playerFighter.OnCancelAttack += target.DisabelOutline;

            isSubscribed = enabled;
        }

        private void OnEnable() => health.OnDeath += DisabelOutline;

        private void OnDisable()
        {
            health.OnDeath -= DisabelOutline;
            Subscribe(this, false);
            if (this == playerChosenTarget) playerChosenTarget = null;
        }
    }
}