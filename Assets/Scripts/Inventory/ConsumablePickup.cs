using UnityEngine;

namespace RPG.Inventory
{
    public class ConsumablePickup : MonoBehaviour, ICollectabele
    {
        [SerializeField] private ConsumableType consumable;
        [SerializeField] private int amountCollected  = 1;
        private InventoryData inventoryData;

        public ConsumableType Consumable { get => consumable; set => consumable = value; }

        private void Awake() => inventoryData = FindObjectOfType<InventoryData>();

        public void PickUpItem() => inventoryData.ColectConsumable(consumable, amountCollected);
    }
}
