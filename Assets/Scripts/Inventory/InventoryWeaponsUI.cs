using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using RPG.Combat;

namespace RPG.Inventory
{
    public class InventoryWeaponsUI : MonoBehaviour
    {
        [SerializeField] private StatsDisplay statsDisplay = null;
        [SerializeField] private float statsDisplayXSpase, statsDisplayYSpase;
        [SerializeField] private InventorySlotDisplay invetoryPrefab = null;
        [SerializeField] UnityEvent HoverEvent, DragEvent, DropEvent, OnClickEvent;
        private InventoryData inventoryData;
        private MouseOnInventoryData mouseData = null;
        private Fighter fighter = null;
        private Dictionary<InventorySlotDisplay, InventorySlotData> slotsOnInterface = new Dictionary<InventorySlotDisplay, InventorySlotData>();
        public event Action<bool> RemovePlayerControl;

        private void Awake()
        {
            mouseData = new MouseOnInventoryData();
            inventoryData = GetComponent<InventoryData>();
        }

        void Start()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();

            foreach (InventorySlotData slotData in inventoryData.InventoryWeaponSlots)
            {
                InventorySlotDisplay slotDisplay = Instantiate(invetoryPrefab, transform);
                slotData.OnSlotUpdate += slotDisplay.SetItemDisplay;
                slotData.UpdateSlotStats(slotData.WeaponConfig);

                slotsOnInterface.Add(slotDisplay, slotData);

                slotDisplay.AddEvent(EventTriggerType.PointerEnter, Action => OnEnter(slotDisplay));
                slotDisplay.AddEvent(EventTriggerType.PointerExit, Action => OnExit(slotDisplay));
                slotDisplay.AddEvent(EventTriggerType.BeginDrag, Action => OnDragStart(slotDisplay));
                slotDisplay.AddEvent(EventTriggerType.EndDrag, Action => OnDragEnd(slotDisplay));
                slotDisplay.AddEvent(EventTriggerType.Drag, Action => OnDrag(slotDisplay));
                slotDisplay.AddEvent(EventTriggerType.PointerClick, Action => OnClick(fighter, slotDisplay));
            }

            HighlightCurrentWeaponDisplay();
        }

        private void OnClick(Fighter fighter, InventorySlotDisplay inventorySlot)
        {
            if (slotsOnInterface[inventorySlot].WeaponConfig)
            {
                OnClickEvent?.Invoke();
                fighter.EquipWeapon(slotsOnInterface[inventorySlot].WeaponConfig);
                HighlightCurrentWeaponDisplay();
            }
        }

        private void OnEnter(InventorySlotDisplay slotDisplay)
        {
            HoverEvent?.Invoke();
            slotDisplay.SlotHover(true);

            if (slotsOnInterface.ContainsKey(slotDisplay))
            {
                mouseData.slotHoveredOver = slotDisplay;

                if (slotsOnInterface[slotDisplay].WeaponConfig)
                    ShowStatDisplay(slotDisplay.transform.position, slotsOnInterface[slotDisplay].WeaponConfig);
            }
        }

        private void OnExit(InventorySlotDisplay slotDisplay)
        {
            slotDisplay.SlotHover(false);

            if (slotsOnInterface.ContainsKey(slotDisplay)) mouseData.slotHoveredOver = null;

            statsDisplay.gameObject.SetActive(false);
        }

        private void OnDrag(InventorySlotDisplay inventorySlot)
        {
            if (mouseData.tempItemBeingDragged != null)
            {
                mouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
                RemovePlayerControl?.Invoke(false);
            }

            if (!slotsOnInterface[inventorySlot].WeaponConfig) return;

            inventorySlot.ItemDisplay.color = new Color(1, 1, 1, 0.3f);
        }

        private void OnDragEnd(InventorySlotDisplay inventorySlot)
        {
            Destroy(mouseData.tempItemBeingDragged);
            RemovePlayerControl?.Invoke(true);

            if (!slotsOnInterface[inventorySlot].WeaponConfig) return;

            inventorySlot.ItemDisplay.color = new Color(1, 1, 1, 1f);

            if (!mouseData.slotHoveredOver) return;

            InventorySlotData mouseHoverSlotData = slotsOnInterface[mouseData.slotHoveredOver];
            WeaponConfig storeConfig = mouseHoverSlotData.WeaponConfig;
            mouseHoverSlotData.UpdateSlotStats(slotsOnInterface[inventorySlot].WeaponConfig);
            slotsOnInterface[inventorySlot].UpdateSlotStats(storeConfig);

            DropEvent?.Invoke();

            if (slotsOnInterface[mouseData.slotHoveredOver].WeaponConfig != fighter.CurrentWeaponConfig && slotsOnInterface[inventorySlot].WeaponConfig != fighter.CurrentWeaponConfig) return;

            HighlightCurrentWeaponDisplay();
        }

        private void OnDragStart(InventorySlotDisplay slotDisplay)
        {
            if (slotsOnInterface[slotDisplay].Name == null || slotsOnInterface[slotDisplay].Name == "") return;

            DragEvent?.Invoke();
            mouseData.tempItemBeingDragged = CreateTempItem(slotDisplay);
        }

        private void HighlightCurrentWeaponDisplay()
        {
            foreach (KeyValuePair<InventorySlotDisplay, InventorySlotData> slot in slotsOnInterface)
            {
                if (slot.Value.WeaponConfig == fighter.CurrentWeaponConfig) slot.Key.ChangeBackgroud(true);
                else slot.Key.ChangeBackgroud(false);
            }
        }

        private GameObject CreateTempItem(InventorySlotDisplay slotDisplay)
        {
            GameObject tempItem = new GameObject();
            RectTransform rt = tempItem.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(100, 100);
            tempItem.transform.SetParent(transform.parent.parent.parent);
            Image img = tempItem.AddComponent<Image>();
            img.sprite = slotDisplay.SlotImage;
            img.raycastTarget = false;

            return tempItem;
        }

        private void ShowStatDisplay(Vector3 slotDisplayPosition, WeaponConfig weaponConfig)
        {
            statsDisplay.gameObject.SetActive(true);
            statsDisplay.transform.position = new Vector3(slotDisplayPosition.x + statsDisplayXSpase, slotDisplayPosition.y + statsDisplayYSpase, 0);
            statsDisplay.DamageText.text = string.Concat("Damage: ", weaponConfig.WeaponDamage);
            statsDisplay.Range.text = string.Concat("Range: ", weaponConfig.WeaponRange);
            statsDisplay.BonusDamage.text = string.Concat("Damage Bonus: ", weaponConfig.DamagePercentageBonus, "%");
            statsDisplay.AttackRate.text = string.Concat("Attack Rate: ", weaponConfig.TimeBetweenAttacks, "s");
        }
    }
}