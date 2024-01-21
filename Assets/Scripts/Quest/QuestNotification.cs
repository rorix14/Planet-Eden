using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using RPG.Inventory;

namespace RPG.Quest
{
    public class QuestNotification : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI title = null;
        [SerializeField] private TextMeshProUGUI status = null;
        [SerializeField] private LeanTweenType esaingType;
        [SerializeField] private UnityEvent OnStartQuest, OnItemColected, OnCompleateQuest;
        private CanvasGroup canvasGroup = null;
        private Queue<IEnumerator> questsToShow = new Queue<IEnumerator>();
        private bool isRunningRotine = false;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            GetComponent<CanvasGroup>().alpha = 0;
        }

        public void StartPopupQue(QuestItem questItem)
        {
            questsToShow.Enqueue(StartPopup(questItem.Quest.QuestTitle, questItem.Quest.GetStatusDescreption(questItem.ItemState), questItem.ItemState));

            if (questItem.ItemState == QuestItemState.DELIVERED)
                questsToShow.Enqueue(StartPopup("You Recieved", StringfyReward(questItem.ChosenRewad), QuestItemState.NOT_ACTIVE));

            if (isRunningRotine == true) return;

            isRunningRotine = true;
            StartCoroutine(questsToShow.Peek());
        }

        private IEnumerator StartPopup(string questTitle, string questStatus, QuestItemState questItemState)
        {
            title.text = questTitle;
            status.text = questStatus;
            EnableNotification(true);

            float faddeTime = 2f;
            float visibleTime = 4f;
            switch (questItemState)
            {
                case QuestItemState.CAN_COLLECT:
                    OnStartQuest?.Invoke();
                    break;
                case QuestItemState.WAS_COLLECTED:
                    OnItemColected?.Invoke();
                    break;
                case QuestItemState.DELIVERED:
                    faddeTime = 1f;
                    visibleTime = 1.5f;
                    OnCompleateQuest?.Invoke();
                    break;
                default:
                    faddeTime = 1f;
                    visibleTime = 3.5f;
                    break;
            }

            yield return FadeRoutine(1, faddeTime);

            yield return new WaitForSeconds(visibleTime);

            yield return FadeRoutine(0, faddeTime);
            EnableNotification(false);

            questsToShow.Dequeue();
            if (questsToShow.Count > 0) yield return StartCoroutine(questsToShow.Peek());

            isRunningRotine = false;
        }

        private IEnumerator FadeRoutine(float target, float time)
        {
            while (!Mathf.Approximately(canvasGroup.alpha, target))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / time);
                yield return null;
            }
            yield return null;
        }

        private void EnableNotification(bool enable)
        {
            title.gameObject.SetActive(enable);
            status.gameObject.SetActive(enable);
        }

        private string StringfyReward(Reward[] rewards)
        {
            string rewardText = "";
            int index = 0;
            foreach (Reward reward in rewards)
            {
                rewardText += GetCosumable(reward.consumableType, reward.amount);
                rewardText += index < rewards.Length - 1 ? ", " : "";
                index++;
            }

            return rewardText;
        }

        private string GetCosumable(ConsumableType consumableType, int amount)
        {
            string consumable = amount + " ";
            switch (consumableType)
            {
                case ConsumableType.HEALTH_POTION:
                    consumable += "Health Item";
                    break;
                case ConsumableType.ARROW:
                    consumable += "Lazer Bolt";
                    break;
                case ConsumableType.FIREBALL:
                    consumable += "Energy Ball";
                    break;
            }

            return consumable += amount > 1 ? "s" : "";
        }  
    }
}