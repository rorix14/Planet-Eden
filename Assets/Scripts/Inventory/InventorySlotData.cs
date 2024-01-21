using UnityEngine;
using System;
using RPG.Combat;

namespace RPG.Inventory
{
    [Serializable]
    public class InventorySlotData
    {
        [SerializeField] private string name = "";
        private WeaponConfig weaponConfig = null;
        public event Action<Sprite, Sprite> OnSlotUpdate;

        public string Name => name;
        public WeaponConfig WeaponConfig => weaponConfig;
        
        public void UpdateSlotStats(WeaponConfig weaponConfig)
        {
            this.weaponConfig = weaponConfig;
            name = weaponConfig ? weaponConfig.name : "";
            OnSlotUpdate?.Invoke(weaponConfig ? weaponConfig.ImageUI : null, weaponConfig ? weaponConfig.SelectedImage : null);
        }
    }
}