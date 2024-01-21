using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RPG.Quest
{
    public class DialogCanvas : MonoBehaviour
    {
        [SerializeField] private GameObject dialogBox;
        [SerializeField] private RectTransform answerConteiner;
        [SerializeField] private GameObject answerContent;
        [SerializeField] public TextMeshProUGUI npcDialog;
        [SerializeField] private AnswerButton answerBtnPrefab;
        [SerializeField] private LeanTweenType easingIn;
        [SerializeField] private LeanTweenType easingOut;
        [SerializeField] private LeanTweenType answerConteinerEase;
        [SerializeField] private float dialogBoxEasingTime = 0.8f;
        [SerializeField] private float anwerBoxEasingTime = 0.5f;

        private List<AnswerButton> answerButtonsPool = new List<AnswerButton>();
        public Transform AnswerContent => answerContent.transform;
        public List<AnswerButton> AnswerButtonsPool => answerButtonsPool;
        public AnswerButton AnswerBtnPrefab => answerBtnPrefab;

        private void Awake()
        {
            dialogBox.transform.localScale = Vector3.zero;
            dialogBox.SetActive(false);
            answerConteiner.gameObject.SetActive(false);
            answerConteiner.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0f);
            answerContent.SetActive(false);
        }

        public void SetNpcDialog(string dialog) => npcDialog.text = dialog;

        public void ShowDialogCanvas()
        {
            dialogBox.SetActive(true);
            LeanTween.scale(dialogBox, new Vector3(1, 1, 1), dialogBoxEasingTime).setEase(easingIn).setOnComplete(() =>
            {
                answerConteiner.gameObject.SetActive(true);
                LeanTween.size(answerConteiner, new Vector2(answerConteiner.sizeDelta.x, 413.7f), anwerBoxEasingTime).setEase(answerConteinerEase).setOnComplete(() =>
                {
                    answerContent.SetActive(true);
                });
            });
        }

        public void CloseDialogCanvas()
        {
            answerContent.SetActive(false);
            LeanTween.size(answerConteiner, new Vector2(answerConteiner.sizeDelta.x, 0), anwerBoxEasingTime).setEase(easingOut).setOnComplete(() =>
            {
                answerConteiner.gameObject.SetActive(false);
                LeanTween.scale(dialogBox.gameObject, new Vector3(0, 0, 0), dialogBoxEasingTime / 2).setEase(easingOut).setOnComplete(() => dialogBox.SetActive(false));
            });
        }
    }
}