using UnityEngine;
using RPG.Attributes;

namespace RPG.Inventory
{
    public class HealthPotion : Consumable
    {
        [SerializeField] private float healthRestore = 30f;
        private Health playerHealth;

        protected override void Awake()
        {
            base.Awake();
            playerHealth = player.GetComponent<Health>();
        }

        public override void Collect(int amount)
        {
            totalAmount += amount;
            inventoryUI.UpdateAmountText(this);
        }

        public override bool Consume()
        {
            if (totalAmount <= 0)
            {
                inventoryUI.StartNoMoreItemWarning(this);
                return false;
            }

            playerHealth.PlayHealEffect();
            playerHealth.Heal(healthRestore);
            totalAmount--;
            inventoryUI.UpdateAmountText(this);
            return true;
        }
    }
}