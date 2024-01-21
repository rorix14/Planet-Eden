using System;
using UnityEngine;
using RPG.Saving;
using RPG.Inventory;
using UnityEngine.SceneManagement;

namespace RPG.Quest
{
    public class QuestGiver : MonoBehaviour, ISaveable
    {
        [SerializeField] private QuestItem[] questItems;
        [SerializeField] private int currentItemIndex;
        [SerializeField] private QuestDialogState currentState;
        [SerializeField] public int totalAffiliationPoints = 0;
        [SerializeField] private InteractIndicator newQuestIndicator = null;
        private AffilationLine currentLine = null;
        private (string savabeleId, int scene) id;
       //private bool questCompleated;
        private QuestsManager questsManager = null;
        private QuestGiverInteract questGiverInteract = null;
        private InventoryData inventoryData = null; 
        public static event Action OnquestCompleated;

        public (string savabeleId, int scene) Id => id;
        public AffilationLine CurrentLine => currentLine;
        public bool QuestCompleated => currentItemIndex == questItems.Length - 1 && currentState == QuestDialogState.POSTCOMPLETION;
        public QuestDialogState CurrentState { get => currentState; set => currentState = value; }
        public int CurrentItemIndex { set => currentItemIndex = value; }

        private void Awake()
        {
            questsManager = FindObjectOfType<QuestsManager>();
            inventoryData = FindObjectOfType<InventoryData>();
            id = (GetComponent<SaveableEntity>().UniqueIdentifier, SceneManager.GetActiveScene().buildIndex);
            questGiverInteract = GetComponent<QuestGiverInteract>();
            questsManager.AddQuestItem(id, currentItemIndex, questItems, currentState);
        }

        private void Update()
        {
            if(currentState == QuestDialogState.WAITING || currentState == QuestDialogState.POSTCOMPLETION)
            {
                newQuestIndicator.gameObject.SetActive(false);
                return;
            }

            switch (currentState)
            {
                case QuestDialogState.INITIAL:
                    newQuestIndicator.Color = Color.red;
                    break;
                case QuestDialogState.COMPLEATED:
                    newQuestIndicator.Color = Color.green;
                    break;
            }

            newQuestIndicator.gameObject.SetActive(true);
        }

        public void UpdateState(int answerIndex)
        {
            currentLine.answerIndex = answerIndex;
            int lastStateIndex = (int)currentState;

            currentLine = GetNextQuestGiverLine();

            if (currentLine == null)
            {
                if (lastStateIndex == (int)QuestDialogState.COMPLEATED)
                {
                    if (currentItemIndex != questItems.Length - 1)
                    {
                        currentItemIndex++;
                        currentState = QuestDialogState.INITIAL;
                    }
                    else
                    {
                        //questCompleated = true;
                        OnquestCompleated?.Invoke();
                    }
                }

                questsManager.ChangeQuestGiverState(id, currentItemIndex, currentState);
                questGiverInteract.SetAffiliationAniation(totalAffiliationPoints);
                questGiverInteract.Cancel();
                return;
            }

            questGiverInteract.QuestGiverDialog.DialogCanvas.SetNpcDialog(currentLine.text);
            questGiverInteract.QuestGiverDialog.ManageAnswerButtons();
        }

        public AffilationLine GetFirstQuestGiverLine() => currentLine = GetCurrentQuest().FirstLine(currentState, totalAffiliationPoints);

        public AffilationLine GetNextQuestGiverLine() => GetCurrentQuest().NextLine(ref currentState, ref totalAffiliationPoints, OnQuestActivated, OnItemDelivered);

        public QuestDialog GetCurrentQuest() => questItems[currentItemIndex].Quest;

        private void OnQuestActivated() => questsManager.CanBeCollected(id);

        private void OnItemDelivered(Reward[] rewards)
        {
            foreach (Reward reward in rewards) inventoryData.ColectConsumable(reward.consumableType, reward.amount);

            questsManager.DeliveredItem(id, rewards);
        }

        [Serializable]
        private struct QuestGiverSavables
        {
            public int affiliationValue;
            //public int currentItemIndex;
            //public bool questCompleated;
        }

        public object CaptureState()
        {
            return new QuestGiverSavables()
            {
                affiliationValue = totalAffiliationPoints,
                //currentItemIndex = currentItemIndex,
                //questCompleated = questCompleated
            };
        }

        public void RestoreState(object state)
        {
            QuestGiverSavables nPCSavables = (QuestGiverSavables)state;
            totalAffiliationPoints = nPCSavables.affiliationValue;
            //currentItemIndex = nPCSavables.currentItemIndex;
            //questCompleated = nPCSavables.questCompleated;
        }
    }
}