using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using RPG.Utils;
using RPG.Core;

public class QuestUILogController : MonoBehaviour
{
    [SerializeField] private GameObject questLogButton = null;
    [SerializeField] private GameObject backGround = null;
    [SerializeField] private GameObject questLogsDisplay = null;
    [SerializeField] private UnityEvent HoverEvent, OnOpen, OnClose;
    [SerializeField] private LeanTweenType easeType;
    private Scrollbar questLogsscrollbar = null;
    private CameraMovement cameraMovement = null;

    private void Awake()
    {
        questLogsscrollbar = questLogsDisplay.GetComponent<ScrollRect>().verticalScrollbar;
        cameraMovement = FindObjectOfType<CameraMovement>();
        DisabelQuestLogDisplay();
    }

    private void Start()
    {
        AddEvent(EventTriggerType.PointerClick, Action => ToggleQuestLogDisplay(), questLogButton);
        AddEvent(EventTriggerType.PointerEnter, Action => OnPointerEnter(), questLogButton);
        AddEvent(EventTriggerType.PointerExit, Action => OnPointerExit(), questLogButton);

        AddEvent(EventTriggerType.PointerClick, Action => ToggleQuestLogDisplay(), backGround);
    }

    private void ToggleQuestLogDisplay()
    {
        if (!questLogsDisplay) return;

        questLogsDisplay.SetActive(!questLogsDisplay.activeSelf);
        backGround.SetActive(!backGround.activeInHierarchy);
        cameraMovement.UIElementStatus(this, backGround.activeInHierarchy);

        RectTransform questDisplayTransform = questLogsDisplay.GetComponent<RectTransform>();

        if (questLogsDisplay.activeSelf)
        {
            OnOpen?.Invoke();

            LeanTween.size(questDisplayTransform, new Vector2(1432.7f, 887.6f), 0.5f).setEase(easeType);
            LeanTween.moveLocal(questLogsDisplay, new Vector3(-1.2398e-05f, 55f, 0f), 0.5f).setEase(easeType).setOnComplete(() =>
            {
                questDisplayTransform.SetChildrenActivation(true);
                questLogsDisplay.GetComponent<ScrollRect>().verticalScrollbar = questLogsscrollbar;
            });
        }
        else
        {
            OnClose?.Invoke();
            DisabelQuestLogDisplay();
        }
    }

    private void DisabelQuestLogDisplay()
    {
        RectTransform questLogsTransform = questLogsDisplay.GetComponent<RectTransform>();

        questLogsTransform.sizeDelta = new Vector2(145.2f, 89.9f);
        questLogsTransform.GetComponent<RectTransform>().localPosition = new Vector3(643.8f, 453.9f, 0f);
        questLogsTransform.GetComponent<RectTransform>().SetChildrenActivation(false);

        questLogsDisplay.GetComponent<ScrollRect>().verticalScrollbar = null;
    }

    private void OnPointerEnter()
    {
        LeanTween.scale(questLogButton, new Vector3(1.1f, 1.1f, 1.1f), 0.3f);
        questLogButton.GetComponent<Image>().color = new Color(1, 1, 1);
        HoverEvent?.Invoke();
    }

    private void OnPointerExit()
    {
        LeanTween.scale(questLogButton, new Vector3(1f, 1f, 1f), 0.3f);
        questLogButton.GetComponent<Image>().color = new Color32(168, 168, 168, 255);
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