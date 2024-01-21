using UnityEngine;
using RPG.Combat;

namespace RPG.Inventory
{
    public class Ammo : Consumable
    {
        [SerializeField] private Projectile projectile = null;
        private Fighter playerFighter;
        protected override void Awake()
        {
            base.Awake();
            playerFighter = player.GetComponent<Fighter>();
        }

        private bool HasAmmo(Projectile projectileShoot)
        {
            if (projectile != projectileShoot) return false;

            return Consume();
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

            totalAmount--;
            inventoryUI.UpdateAmountText(this);
            return true;
        }

        private void OnEnable() => playerFighter.HasAmmo += HasAmmo;

        private void OnDisable() => playerFighter.HasAmmo -= HasAmmo;
    }
}