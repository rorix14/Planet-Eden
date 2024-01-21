using UnityEngine;
using RPG.Combat;

namespace RPG.Inventory
{
    public class WepaonPickup : MonoBehaviour, ICollectabele
    {
        [SerializeField] private WeaponConfig weapon = null;
        private InventoryData inventoryData;

        private void Awake() => inventoryData = FindObjectOfType<InventoryData>();

        public void PickUpItem()
        {
            if(weapon) inventoryData.AddWeapon(weapon);
        }      
    }
}
