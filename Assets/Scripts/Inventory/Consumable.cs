using UnityEngine;

namespace RPG.Inventory
{
    public enum ConsumableType
    {
        HEALTH_POTION,
        ARROW,
        FIREBALL,
        NON_CONSUMABLE
    }

    public abstract class Consumable : MonoBehaviour
    {
        [SerializeField] protected ConsumableType consumable;
        [SerializeField] protected float totalAmount = 0f;
        protected InventoryConsumablesUI inventoryUI;
        protected GameObject player;

        public ConsumableType ConsumableType => consumable;
        public float TotalAmount { get => totalAmount; set => totalAmount = value; }

        protected virtual void Awake()
        {
            player = GameObject.FindWithTag("Player");
            inventoryUI = GetComponent<InventoryConsumablesUI>();
        }

        public abstract void Collect(int amount);
        public abstract bool Consume();
    }
}