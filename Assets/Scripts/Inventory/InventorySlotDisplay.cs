using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;
using System.Collections;

namespace RPG.Inventory
{
    public class InventorySlotDisplay : MonoBehaviour
    {
        [SerializeField] private Image itemDisplay = null;
        [SerializeField] private Image border = null;
        [SerializeField] private TextMeshProUGUI amountTextDisplay;
        [SerializeField] private ConsumableType consumable;
        [SerializeField] private float warningTime = 3f;
        private float timeSinceLastActivated = Mathf.Infinity;
        private Coroutine currentActiveRoutine = null;
        private bool isSelected = false;
        private Image backGroud = null;
        private Color initialBackgroudColor;
        private Sprite slotImage = null;
        private Sprite selectedImage = null;
        private Color borderInitialColor;

        public Image ItemDisplay => itemDisplay;
        public Image Border => border;
        public TextMeshProUGUI AmountTextDisplay => amountTextDisplay;
        public ConsumableType ConsumableType => consumable;
        public bool IsSelected => isSelected;
        public Sprite SlotImage => slotImage;

        private void Awake()
        {
            backGroud = GetComponent<Image>();
            initialBackgroudColor = backGroud.color;
            borderInitialColor = border.color;
        }

        private void Update() => timeSinceLastActivated += Time.deltaTime;

        public void ChangeBackgroud(bool hasCurrentWeapon)
        {
            //if (hasCurrentWeapon) backGroud.color = new Color32(14, 180, 155, 255);
            //else backGroud.color = initialBackgroudColor;
            if (hasCurrentWeapon) itemDisplay.sprite = selectedImage;
            else
            {
                itemDisplay.sprite = slotImage;
                itemDisplay.color = new Color(1, 1, 1, SlotImage ? 1 : 0);
            }
        }

        public void SlotHover(bool asMouse)
        {
            //if (asMouse) border.color = new Color(188, 222, 0, 1);
            //else border.color = new Color(0, 0, 0, 1);
            border.enabled = asMouse;
        }

        public void NoMoreItemWarning()
        {
            if (currentActiveRoutine != null) StopCoroutine(currentActiveRoutine);

            currentActiveRoutine = StartCoroutine(WarningRoutine());
        }

        private IEnumerator WarningRoutine()
        {
          
           timeSinceLastActivated = 0;
           border.color = new Color(1, 0, 0, 1);

            while (warningTime > timeSinceLastActivated)
            {
                //border.color = new Color(1, 0, 0, 1);
                border.enabled = true;
                yield return new WaitForSeconds(0.2f);

                //border.color = new Color(1, 1, 1, 1);
                border.enabled = false;
                yield return new WaitForSeconds(0.2f);
            }

            border.color = borderInitialColor;
            yield return null;
        }

        public void SetItemDisplay(Sprite sprite, Sprite selected)
        {
            itemDisplay.sprite = sprite;
            itemDisplay.color = new Color(1, 1, 1, sprite ? 1 : 0);
            selectedImage = selected;
            slotImage = sprite;
        }

        public void AddEvent(EventTriggerType type, UnityAction<BaseEventData> action)
        {
            EventTrigger trigger = GetComponent<EventTrigger>();
            EventTrigger.Entry eventTrigger = new EventTrigger.Entry();
            eventTrigger.eventID = type;
            eventTrigger.callback.AddListener(action);
            trigger.triggers.Add(eventTrigger);
        }
    }
}
