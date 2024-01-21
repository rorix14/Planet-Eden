using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace RPG.SceneManagement
{
    public abstract class Menus : MonoBehaviour
    {
        [SerializeField] protected ConfirmationPopUp confirmationPopUp = null;
        [SerializeField] private MenuButton[] buttons;
        [SerializeField] protected UnityEvent ButtonClickedSound, ConfirmationSound, ButtonHoverSound;
        protected SavingWrapper savingWrapper = null;

        public ConfirmationPopUp ConfirmationPopUp => confirmationPopUp;
        public MenuButton[] Buttons => buttons;

        private void Awake() => savingWrapper = FindObjectOfType<SavingWrapper>();

        protected virtual void Start()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                GameObject button = buttons[i].gameObject;
                CanvasGroup border = button.transform.GetComponentInChildren<CanvasGroup>();
                border.alpha = 0.3f;

                AddEvent(EventTriggerType.PointerClick, Action => MenuButtonClick(button), button);
                AddEvent(EventTriggerType.PointerEnter, Action => OnEnter(border, button), button);
                AddEvent(EventTriggerType.PointerExit, Action => OnExit(border, button), button);
            }
        }

        protected abstract void MenuButtonClick(GameObject button);

        private void OnEnter(CanvasGroup border, GameObject button)
        {
            AnimateButton(border, button, 1, new Vector3(1.1f, 1.1f, 1));
            ButtonHoverSound?.Invoke();
        }

        private void OnExit(CanvasGroup border, GameObject button) => AnimateButton(border, button, 0.3f,  new Vector3(1, 1, 1));

        protected void ConfirmQuitGame()
        {
            confirmationPopUp.Confirmation = () => Application.Quit();
            confirmationPopUp.PopUpText.text = confirmationPopUp.QuitGameConfirmationText;
            confirmationPopUp.gameObject.SetActive(true);
        }

        private void AnimateButton(CanvasGroup border, GameObject button, float alpha, Vector3 scale)
        {
            LeanTween.alphaCanvas(border, alpha, 0.2f).setIgnoreTimeScale(true);
            LeanTween.scale(button, scale, 0.2f).setIgnoreTimeScale(true);
        }

        private void AddEvent(EventTriggerType type, UnityAction<BaseEventData> action, GameObject button)
        {
            EventTrigger trigger = button.GetComponent<EventTrigger>();
            EventTrigger.Entry eventTrigger = new EventTrigger.Entry();
            eventTrigger.eventID = type;
            eventTrigger.callback.AddListener(action);
            trigger.triggers.Add(eventTrigger);
        }
    }
}