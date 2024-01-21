using UnityEngine;
using RPG.Utils;
using RPG.Movement;

namespace RPG.Control
{
    public class NPCInteract : MonoBehaviour, IRaycastable
    {
        private LazyValue<IInteract> interact = null;
        private PlayerController playerController = null;

        private void Awake() => interact = new LazyValue<IInteract>(() => GetComponent<IInteract>());

        private void Start() => interact.ForceInit();

        public CursorType GetCursorType() => CursorType.Interact;

        public bool HandleRaycast(PlayerController callingController)
        {
            Mover playerMover = callingController.GetComponent<Mover>();

            if (!playerMover.CanMoveTo(transform.position)) return false;

            if (Input.GetMouseButton(0))
            {
                playerController = callingController;

                interact.value.Interact(playerController.gameObject);
            }

            return true;
        }
      
        private void RemovePlayerControl(bool remove) => playerController.enabled = remove;

        private void OnEnable() => interact.value.removeControl += RemovePlayerControl;

        private void OnDisable() => interact.value.removeControl -= RemovePlayerControl;
    }
}