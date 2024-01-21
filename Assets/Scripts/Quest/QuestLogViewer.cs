using System.Collections.Generic;
using UnityEngine;
using System;
using RPG.Utils;

namespace RPG.Quest
{
    public class QuestLogViewer : MonoBehaviour
    {
        [Serializable]
        public class StateIcon
        {
            public QuestItemState itemState;
            public Sprite iconSprite;
            public Sprite border;
        }

        [SerializeField] private GameObject questLogContentConteiner = null;
        [SerializeField] private QuestLog questLogPrefab = null;
        [SerializeField] private QuestNotification questNotification = null;
        [SerializeField] private StateIcon[] statesIcons;
        private LazyValue<QuestsManager> questManager;
        private List<QuestLog> questLogUIElementsList = new List<QuestLog>();
        private List<QuestGiverToItem> questLogList = new List<QuestGiverToItem>();
        private Dictionary<QuestItemState, (Sprite icon, Sprite border)> stateIconLookUp;

        private void Awake() => questManager = new LazyValue<QuestsManager>(() => FindObjectOfType<QuestsManager>());

        void Start()
        {
            questManager.ForceInit();
            BuildQuestLogList();
            UpdateLogs();
        }

        private void BuildQuestLogList()
        {
            //CAN USE THIS INSTED OF CLASS QuestGiverToItem
            //List<QuestItem> test = new List<QuestItem>();
            //test.Sort((firstPair, nextPair) => firstPair.ItemState.CompareTo(nextPair.ItemState));

            questLogList.Clear();

            foreach (KeyValuePair<(string, int), QuestsManager.QuestStructure> dictionaryItem in questManager.value.GetQuestStructureLookUp)
                foreach (QuestItem questItem in dictionaryItem.Value.questItems) AddNewQuestItemData(questItem);
        }

        private void OnQuestItemUpdate(QuestItem questItem)
        {
            if (questLogList == null) BuildQuestLogList();

            if (!HasQuestItemInList(questItem)) AddNewQuestItemData(questItem);

            UpdateLogs();

            questNotification.StartPopupQue(questItem);
        }

        private void AddNewQuestItemData(QuestItem questItem)
        {
            QuestGiverToItem newQuestLog = new QuestGiverToItem(questItem, OnQuestItemUpdate);
            questLogList.Add(newQuestLog);
        }

        private bool HasQuestItemInList(QuestItem questItem)
        {
            foreach (QuestGiverToItem questGiverToItem in questLogList) if (questGiverToItem.questItem == questItem) return true;

            return false;
        }

        private void UpdateLogs()
        {
            questLogList.Sort();

            int index = 0;
            foreach (QuestGiverToItem questLog in questLogList)
            {
                if (questLog.questItem.ItemState == QuestItemState.NOT_ACTIVE) continue;

                if (questLogUIElementsList.Count <= index)
                {
                    QuestLog newQuestLog = Instantiate(questLogPrefab, questLogContentConteiner.transform);
                    questLogUIElementsList.Add(newQuestLog);
                }

                UpdateLogDisplay(questLogUIElementsList[index], questLog);
                index++;
            }
        }

        private void UpdateLogDisplay(QuestLog questLogDisplay, QuestGiverToItem questLogData)
        {
            questLogDisplay.Title.text = questLogData.questItem.Quest.QuestTitle;
            questLogDisplay.Description.text = questLogData.questItem.Quest.QuestDescription;
            questLogDisplay.Status.text = questLogData.questItem.Quest.GetStatusDescreption(questLogData.questItem.ItemState);

            if (stateIconLookUp == null) BuildStateLinesLookUpLookUp();

            if (stateIconLookUp.ContainsKey(questLogData.questItem.ItemState))
            {
                questLogDisplay.StatusIcon.sprite = stateIconLookUp[questLogData.questItem.ItemState].icon;
                questLogDisplay.Border.sprite = stateIconLookUp[questLogData.questItem.ItemState].border;
            }
        }

        private void BuildStateLinesLookUpLookUp()
        {
            stateIconLookUp = new Dictionary<QuestItemState, (Sprite icon, Sprite border)>();

            foreach (StateIcon stateIcon in statesIcons) stateIconLookUp[stateIcon.itemState] = (stateIcon.iconSprite, stateIcon.border);
        }

        public class QuestGiverToItem : IComparable<QuestGiverToItem>
        {
            public QuestItem questItem;

            public QuestGiverToItem(QuestItem questItem, Action<QuestItem> action)
            {
                this.questItem = questItem;
                questItem.OnCollected += action;
            }

            public int CompareTo(QuestGiverToItem other)
            {
                if (other == null) return 0;
                else return questItem.ItemState.CompareTo(other.questItem.ItemState);
            }
        }

        private void OnEnable()
        {
            if (questManager.value != null) questManager.value.OnChangeItemState += OnQuestItemUpdate;
        }

        private void OnDisable()
        {
            if (questManager.value != null) questManager.value.OnChangeItemState -= OnQuestItemUpdate;

            foreach (QuestGiverToItem questLog in questLogList) questLog.questItem.OnCollected -= OnQuestItemUpdate;
        }
    }
}