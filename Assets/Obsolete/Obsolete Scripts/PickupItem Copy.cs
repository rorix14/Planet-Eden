using System.Collections;
using UnityEngine;
using RPG.Inventory;
using RPG.Attributes;
using RPG.Movement;
using RPG.Combat;


// NO LONGER IN USE
/*
namespace RPG.Control
{
    public class PickupItem : MonoBehaviour, IRaycastable
    {
        [SerializeField] WeaponConfig weapon = null;
        [SerializeField] float healthToRestore = 0;
        [SerializeField] float respawnTime = 5f;
        private InventoryData inventoryData;

        private void Awake() => inventoryData = FindObjectOfType<InventoryData>();

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag != "Player") return;
            Pickup(other.gameObject);
        }

        private void Pickup(GameObject fighter)
        {
            if (weapon)
            {
                //fighter.GetComponent<Fighter>().EquipWeapon(weapon);
                inventoryData.AddItem(weapon);
            }

            //if (healthToRestore > 0) fighter.GetComponent<Health>().Heal(healthToRestore);

            StartCoroutine(HideForSeconds(respawnTime));
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

            foreach (Transform child in transform) child.gameObject.SetActive(shouldShow);
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            Mover playerMover = callingController.GetComponent<Mover>();

            if (!playerMover.CanMoveTo(transform.position)) return false;

            if (Input.GetMouseButtonDown(0)) Pickup(callingController.gameObject);

            return true;
        }

        public CursorType GetCursorType() => CursorType.Pickup;
    }
}*/

