using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace RPG.Inventory
{
    public class InventoryConsumablesUI : MonoBehaviour
    {
        [SerializeField] private InventorySlotDisplay[] cosumablesSlotsDisplay;
        [SerializeField] private UnityEvent HoverEvent;
        private Dictionary<InventorySlotDisplay, Consumable> cosumableItemSlots = new Dictionary<InventorySlotDisplay, Consumable>();
        private InventoryData inventoryData;

        private void Awake() => inventoryData = GetComponent<InventoryData>();

        void Start()
        {
            foreach (InventorySlotDisplay slotDisplay in cosumablesSlotsDisplay)
            {
                if (!inventoryData.ConsumablesLookUp.ContainsKey(slotDisplay.ConsumableType)) continue;

                cosumableItemSlots.Add(slotDisplay, inventoryData.ConsumablesLookUp[slotDisplay.ConsumableType]);
                UpdateAmountText(inventoryData.ConsumablesLookUp[slotDisplay.ConsumableType]);

                slotDisplay.AddEvent(EventTriggerType.PointerClick, Action => OnClickConsumableSlot(slotDisplay));
                slotDisplay.AddEvent(EventTriggerType.PointerEnter, Action => OnEnter(slotDisplay));
                slotDisplay.AddEvent(EventTriggerType.PointerExit, Action => OnExit(slotDisplay));
            }
        }

        public void UpdateAmountText(Consumable item)
        {
            foreach (InventorySlotDisplay slotDisplay in cosumableItemSlots.Keys)
            {
                if (cosumableItemSlots[slotDisplay] != item) continue;

                slotDisplay.AmountTextDisplay.text = item.TotalAmount.ToString();
            }
        }

        public void StartNoMoreItemWarning(Consumable item)
        {
            foreach (InventorySlotDisplay slotDisplay in cosumableItemSlots.Keys)
            {
                if (cosumableItemSlots[slotDisplay] != item) continue;

                slotDisplay.NoMoreItemWarning();
            }
        }

        private void OnClickConsumableSlot(InventorySlotDisplay inventorySlot)
        {
            switch (inventorySlot.ConsumableType)
            {
                case ConsumableType.HEALTH_POTION:
                    cosumableItemSlots[inventorySlot].Consume();
                    break;
                case ConsumableType.ARROW:
                    break;
                case ConsumableType.FIREBALL:
                    break;
                case ConsumableType.NON_CONSUMABLE:
                    break;
            }
        }

        private void OnEnter(InventorySlotDisplay slotDisplay)
        {
            HoverEvent?.Invoke();
            //slotDisplay.Border.color = new Color(188, 222, 0, 1);
            slotDisplay.SlotHover(true);
        }

        private void OnExit(InventorySlotDisplay slotDisplay)
        {
            //slotDisplay.Border.color = new Color(0, 0, 0, 1);
            slotDisplay.SlotHover(false);
        }
    }
}
