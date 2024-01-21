using System.Collections.Generic;
using System;
using UnityEngine;
using RPG.Saving;
using RPG.Combat;

namespace RPG.Inventory
{
    public class InventoryData : MonoBehaviour, ISaveable
    {
        [SerializeField] private InventorySlotData[] inventoryWeaponSlots;
        private Dictionary<ConsumableType, Consumable> consumablesLookUp;

        public InventorySlotData[] InventoryWeaponSlots => inventoryWeaponSlots;
        public Dictionary<ConsumableType, Consumable> ConsumablesLookUp => consumablesLookUp;

        private void Awake()
        {
            consumablesLookUp = new Dictionary<ConsumableType, Consumable>();

            foreach (Consumable consumable in GetComponents<Consumable>()) consumablesLookUp.Add(consumable.ConsumableType, consumable);
        }

        public void ColectConsumable(ConsumableType consumable, int amount)
        {
            if (!consumablesLookUp.ContainsKey(consumable)) return;

            consumablesLookUp[consumable].Collect(amount);
        }

        public void AddWeapon(WeaponConfig weaponConfig)
        {
            if (EmpySlotCount() <= 0) return;

            InventorySlotData slot = FindItemOnInventory(weaponConfig);

            if (slot == null)
            {
                SetEmptySlot(weaponConfig);
                return;
            }

            slot.UpdateSlotStats(weaponConfig);
        }

        private void SetEmptySlot(WeaponConfig weaponConfig)
        {
            foreach (InventorySlotData slot in inventoryWeaponSlots)
            {
                if (slot.Name == null || slot.Name == "")
                {
                    slot.UpdateSlotStats(weaponConfig);
                    return;
                }
            }
        }

        private int EmpySlotCount()
        {
            int counter = 0;
            foreach (InventorySlotData slot in inventoryWeaponSlots) if (slot.Name == null || slot.Name == "") counter++;

            return counter;
        }

        private InventorySlotData FindItemOnInventory(WeaponConfig weaponConfig)
        {
            foreach (InventorySlotData slot in inventoryWeaponSlots) if (slot.Name == weaponConfig.name) return slot;

            return null;
        }

        [Serializable]
        public struct InvetorySaveData
        {
            public string[] inventoryWeapons;
            public ConsumableData[] consumablesData;
        }

        [Serializable]
        public struct ConsumableData
        {
            public ConsumableType consumableReference;
            public float amount;
        }

        private string[] WeaponsIDs()
        {
            string[] inventorySlotsDataReference = new string[inventoryWeaponSlots.Length];
            for (int i = 0; i < inventoryWeaponSlots.Length; i++) inventorySlotsDataReference[i] = inventoryWeaponSlots[i].Name;
            return inventorySlotsDataReference;
        }

        private ConsumableData[] ConsumableAmmouts()
        {
            ConsumableData[] consumablesData = new ConsumableData[ConsumablesLookUp.Count];

            int index = 0;
            foreach (ConsumableType consumable in ConsumablesLookUp.Keys)
            {
                consumablesData[index] = new ConsumableData()
                {
                    consumableReference = consumable,
                    amount = ConsumablesLookUp[consumable].TotalAmount
                };
                index++;
            }

            return consumablesData;
        }

        public object CaptureState()
        {
            var data = new InvetorySaveData()
            {
                inventoryWeapons = WeaponsIDs(),
                consumablesData = ConsumableAmmouts()
            };

            return data;
        }

        public void RestoreState(object state)
        {
            InvetorySaveData invetorySaveData = (InvetorySaveData)state;

            string[] inventorySlotsDataReference = invetorySaveData.inventoryWeapons;
            for (int i = 0; i < inventorySlotsDataReference.Length; i++)
            {
                WeaponConfig weapon = Resources.Load<WeaponConfig>(inventorySlotsDataReference[i]);
                inventoryWeaponSlots[i].UpdateSlotStats(weapon);
            }

            foreach (ConsumableData consumableData in invetorySaveData.consumablesData)
            {
                if (ConsumablesLookUp.ContainsKey(consumableData.consumableReference))
                    ConsumablesLookUp[consumableData.consumableReference].TotalAmount = consumableData.amount;
            }
        }
    }
}