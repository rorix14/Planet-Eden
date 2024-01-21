using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using RPG.Inventory;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;

namespace RPG.Control
{
    public class PickupItem : MonoBehaviour, IRaycastable, IAction, ISaveable
    {
        [SerializeField] private float respawnTime = 5f;
        [SerializeField] private GameObject[] toDisable = null;
        [SerializeField] private UnityEvent OnPickUp = null;
        private ICollectabele collectabele = null;
        private Mover playerMover = null;
        private bool wasPickedUp = false;

        private void Awake() => collectabele = GetComponent<ICollectabele>();

        private void Start() 
        {
            if (wasPickedUp) ShowPickup(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag != "Player" || wasPickedUp) return;

            if (collectabele != null) collectabele.PickUpItem();

            wasPickedUp = true;
            OnPickUp?.Invoke();

            ShowPickup(false);
            //StartCoroutine(HideForSeconds(respawnTime));
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        private void ShowPickup(bool shouldShow)
        {
            GetComponent<SphereCollider>().enabled = shouldShow;

            foreach (GameObject child in toDisable) child.SetActive(shouldShow);
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            playerMover = callingController.GetComponent<Mover>();

            if (!playerMover.CanMoveTo(transform.position)) return false;

            if (Input.GetMouseButton(0))
            {
                playerMover.MoveTo(transform.position, 1);
                callingController.GetComponent<ActionScheduler>().StarAction(this);
            }

            return true;
        }

        public CursorType GetCursorType() => CursorType.Pickup;

        public void Cancel()
        {
            playerMover?.Cancel();
            playerMover = null;
        }

        public object CaptureState() => wasPickedUp;
       
        public void RestoreState(object state) => wasPickedUp = (bool)state;
    }
}