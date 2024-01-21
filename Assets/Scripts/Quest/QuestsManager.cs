using System;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Quest
{
    public class QuestsManager : MonoBehaviour, ISaveable
    {
        public event Action<QuestItem> OnChangeItemState;

        public class QuestStructure
        {
            public (string id, int scene) id;
            public int currentItemIndex;
            public List<QuestItem> questItems;
            public QuestDialogState questgiverState;

            public QuestStructure((string id, int scene) id, int currentItemIndex, List<QuestItem> questItems, QuestDialogState questgiverState)
            {
                this.id = id;
                this.currentItemIndex = currentItemIndex;
                this.questItems = questItems;
                this.questgiverState = questgiverState;
            }
        }

        Dictionary<(string id, int scene), QuestStructure> QuestStructureLookUp = new Dictionary<(string id, int scene), QuestStructure>();

        public Dictionary<(string id, int scene), QuestStructure> GetQuestStructureLookUp => QuestStructureLookUp;

        public void AddQuestItem((string id, int scene) questGiver, int currentItemIndex, QuestItem[] questItems, QuestDialogState questgiverState)
        {
            if (QuestStructureLookUp.ContainsKey(questGiver)) return;

            QuestStructureLookUp[questGiver] = new QuestStructure(questGiver, currentItemIndex, new List<QuestItem>(), questgiverState);

            foreach (QuestItem questItem in questItems)
            {
                QuestItem questobj = Instantiate(questItem, transform);
                questobj.name = questItem.name;
                questobj.gameObject.SetActive(false);
                QuestStructureLookUp[questGiver].questItems.Add(questobj);
            }
        }

        public void CanBeCollected((string id, int scene) questGiver)
        {
            QuestItem questItemToUpdate = QuestStructureLookUp[questGiver].questItems[QuestStructureLookUp[questGiver].currentItemIndex];
            questItemToUpdate.ItemState = QuestItemState.CAN_COLLECT;
            OnChangeItemState?.Invoke(questItemToUpdate);
        }

        public void ChangeQuestGiverState((string id, int scene) questGiver, int currentItemIndex, QuestDialogState questState)
        {
            QuestStructureLookUp[questGiver].currentItemIndex = currentItemIndex;
            QuestStructureLookUp[questGiver].questgiverState = questState;
        }

        public void DeliveredItem((string id, int scene) questGiver, Reward[] rewards)
        {
            QuestItem questItemToUpdate = QuestStructureLookUp[questGiver].questItems[QuestStructureLookUp[questGiver].currentItemIndex];
            questItemToUpdate.ItemState = QuestItemState.DELIVERED;
            questItemToUpdate.ChosenRewad = rewards;
            OnChangeItemState?.Invoke(questItemToUpdate);
        }

        public void ResetQuestManager()
        {
            foreach (QuestStructure questStructure in QuestStructureLookUp.Values)
            {
                questStructure.questItems.ForEach((itemToDestroy) =>
                {
                    Destroy(itemToDestroy.gameObject);
                    itemToDestroy = null;
                });
            }

            QuestStructureLookUp.Clear();
        }

        [Serializable]
        private struct QuestStructureValues
        {
            public (string id, int scene) id;
            public int currentItemIndex;
            public List<QuestItemSaveValues> questItemSaveVlues;
            public QuestDialogState questgiverState;

            public QuestStructureValues(QuestStructure questStructure)
            {
                questItemSaveVlues = new List<QuestItemSaveValues>();
                foreach (QuestItem questItem in questStructure.questItems) questItemSaveVlues.Add(new QuestItemSaveValues(questItem));

                id = questStructure.id;
                currentItemIndex = questStructure.currentItemIndex;
                questgiverState = questStructure.questgiverState;
            }
        }

        [Serializable]
        private struct QuestItemSaveValues
        {
            public string questItemResourceName;
            public QuestItemState itemState;
            public Reward[] rewards;

            public QuestItemSaveValues(QuestItem questItem)
            {
                questItemResourceName = questItem.name;
                itemState = questItem.ItemState;
                rewards = questItem.ChosenRewad;
            }
        }

        public object CaptureState()
        {
            List<QuestStructureValues> questStructures = new List<QuestStructureValues>();

            foreach (QuestStructure value in QuestStructureLookUp.Values)
            {
                QuestStructureValues tempQuestStructValues = new QuestStructureValues(value);

                questStructures.Add(tempQuestStructValues);
            }

            return questStructures;
        }

        public void RestoreState(object state)
        {
            // LOAD DATA FROM SAVE FILE AND UPDATE QUEST ITEMS VALUES 
            List<QuestStructureValues> questStructureValues = (List<QuestStructureValues>)state;

            foreach (QuestStructureValues questStructureValue in questStructureValues)
            {
                if (!QuestStructureLookUp.ContainsKey(questStructureValue.id))
                {
                    List<QuestItem> questItems = new List<QuestItem>();
                    foreach (QuestItemSaveValues questItemSaveValue in questStructureValue.questItemSaveVlues)
                    {
                        QuestItem questItem = Instantiate(Resources.Load<QuestItem>(questItemSaveValue.questItemResourceName), transform);
                        UpdeteQuestItemValues(questItem, questItemSaveValue);
                        questItem.gameObject.SetActive(false);

                        questItems.Add(questItem);
                    }

                    QuestStructure tempQuestStructure =
                        new QuestStructure(questStructureValue.id, questStructureValue.currentItemIndex, questItems, questStructureValue.questgiverState);
                    QuestStructureLookUp[questStructureValue.id] = tempQuestStructure;
                }
                else
                {
                    for (int i = 0; i < questStructureValue.questItemSaveVlues.Count; i++)
                        UpdeteQuestItemValues(QuestStructureLookUp[questStructureValue.id].questItems[i], questStructureValue.questItemSaveVlues[i]);

                    QuestStructureLookUp[questStructureValue.id].currentItemIndex = questStructureValue.currentItemIndex;
                    QuestStructureLookUp[questStructureValue.id].questgiverState = questStructureValue.questgiverState;
                }
            }

            // MANAGE QUEST ITEMS SPAWN 
            foreach (QuestStructure questStructure in QuestStructureLookUp.Values)
            {
                foreach (QuestItem questItem in questStructure.questItems)
                {
                    if ((int)questItem.SceneToSpwan == SceneManager.GetActiveScene().buildIndex &&
                        (questItem.ItemState != QuestItemState.WAS_COLLECTED && questItem.ItemState != QuestItemState.DELIVERED))
                    {
                        questItem.transform.position = questItem.SpawnPlace;
                        questItem.gameObject.SetActive(true);
                        if (questItem.ItemState == QuestItemState.CAN_COLLECT) questItem.EnableProperties(true);
                    }
                    else questItem.gameObject.SetActive(false);
                }
            }

            // UPDATE QUEST GIVER VALUES
            QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>();

            foreach ((string id, int scene) uid in QuestStructureLookUp.Keys)
            {
                foreach (QuestItem questItem in QuestStructureLookUp[uid].questItems)
                {
                    foreach (QuestGiver questGiver in questGivers)
                    {
                        if (uid != questGiver.Id) continue;

                        if (questItem.ItemState == QuestItemState.WAS_COLLECTED && QuestStructureLookUp[uid].questgiverState == QuestDialogState.WAITING)
                            QuestStructureLookUp[uid].questgiverState = QuestDialogState.COMPLEATED;

                        questGiver.CurrentItemIndex = GetQuestStructureLookUp[uid].currentItemIndex;
                        questGiver.CurrentState = QuestStructureLookUp[uid].questgiverState;
                    }
                }
            }
        }

        private void UpdeteQuestItemValues(QuestItem questItem, QuestItemSaveValues itemSaveValues)
        {
            questItem.name = itemSaveValues.questItemResourceName;
            questItem.ItemState = itemSaveValues.itemState;
            questItem.ChosenRewad = itemSaveValues.rewards;
        }
    }
}

// DESTROY ANY QUEST ITEM THAT WAS NOT SAVED
//List<string> questGiverToRemove = new List<string>();
//foreach (string questGiverId in QuestStructureLookUp.Keys)
//{
//    bool foudQuestGiver = false;
//    foreach (QuestStructureValues questStructureValue in questStructureValues)
//    {
//        if (questStructureValue.id == questGiverId)
//        {
//            foudQuestGiver = true;
//            break;
//        }
//    }

//    if (!foudQuestGiver) questGiverToRemove.Add(questGiverId);
//}

//foreach (string questGiver in questGiverToRemove)
//{
//    QuestStructureLookUp[questGiver].questItems.ForEach((itemToDestroy) =>
//    {
//        Destroy(itemToDestroy.gameObject);
//        itemToDestroy = null;
//    });

//    QuestStructureLookUp.Remove(questGiver);
//}


//List<QuestItem> itemsToDestroy = new List<QuestItem>();
//foreach (QuestStructure questStructure in QuestStructureLookUp.Values)
//{
//    foreach (QuestItem questItem in questStructure.questItems)
//    {
//        bool itemFound = false;
//        foreach (QuestStructureValues questStructureValue in questStructureValues)
//        {
//            foreach (QuestItemSaveValues questItemSaveValue in questStructureValue.questItemSaveVlues)
//            {
//                if (questItemSaveValue.questItemResourceName == questItem.name)
//                {
//                    itemFound = true;
//                    break;
//                }
//            }

//            if (itemFound == true) break;
//        }

//        if (itemFound == false) itemsToDestroy.Add(questItem);
//    }
//}

//itemsToDestroy.ForEach((itemToDestroy) =>
//{
//    foreach (QuestStructure questStructure in QuestStructureLookUp.Values) questStructure.questItems.Remove(itemToDestroy);

//    Destroy(itemToDestroy.gameObject);
//    itemToDestroy = null;
//});

//private void OnEnable()
//{
//    SceneManager.sceneLoaded += SpawnQuestItem;
//    SceneManager.sceneLoaded += SetQuestGiverState;
//}

//private void OnDisable()
//{
//    SceneManager.sceneLoaded -= SpawnQuestItem;
//    SceneManager.sceneLoaded -= SetQuestGiverState;
//}

//private void SetQuestGiverState(Scene scene, LoadSceneMode loadSceneMode)
//{
//    foreach (string uid in QuestStructureLookUp.Keys)
//    {
//        foreach (QuestItem questItem in QuestStructureLookUp[uid].questItems)
//        {
//            if (questItem.WasCollected)
//            {
//                foreach (QuestGiver questGiver in FindObjectsOfType<QuestGiver>())
//                {
//                    if (uid != questGiver.id) continue;

//                    questGiver.OnQuestItemCollected(QuestStructureLookUp[uid].questgiverState);
//                }
//            }
//        }
//    }
//}

//private void SpawnQuestItem(Scene scene, LoadSceneMode loadSceneMode)
//{
//    foreach (QuestStructure questStructure in QuestStructureLookUp.Values)
//    {
//        foreach (QuestItem questItem in questStructure.questItems)
//        {
//            if (questItem.SceneToSpwan == SceneManager.GetActiveScene().buildIndex && !questItem.WasCollected)
//            {
//                questItem.transform.position = questItem.SpawnPlace;
//                questItem.gameObject.SetActive(true);
//            }
//            else questItem.gameObject.SetActive(false);
//        }
//    }
//}

//Dictionary<string, QuestStructure> QuestItemsPool = new Dictionary<string, List<QuestItem>>();

//public void AddQuestItem(string questGiver, QuestItem questItem, int totalQuestNumber)
//{
//    if (!QuestItemsPool.ContainsKey(questGiver)) QuestItemsPool[questGiver] = new List<QuestItem>();

//    AddToList(QuestItemsPool[questGiver], questItem, totalQuestNumber);
//}

//public void ChangeCompleted(string questGiver, int index) => QuestItemsPool[questGiver][index].CanCollect = true;

//private void AddToList(List<QuestItem> questItems, QuestItem questItem, int totalQuestNumber)
//{
//    if (questItems.Count < totalQuestNumber)
//    {
//        QuestItem questobj = Instantiate(questItem, transform);
//        questobj.gameObject.SetActive(false);
//        questItems.Add(questobj);
//    }
//}

//private void SpawnQuestItem(Scene scene, LoadSceneMode loadSceneMode)
//{
//    foreach (List<QuestItem> questItems in QuestItemsPool.Values)
//    {
//        foreach (QuestItem questItem in questItems)
//        {
//            if (questItem.sceneToSpwan == SceneManager.GetActiveScene().buildIndex && !questItem.wasCollected)
//            {
//                questItem.transform.position = questItem.spawnPlace;
//                questItem.gameObject.SetActive(true);
//            }
//            else questItem.gameObject.SetActive(false);
//        }
//    }
//}

//private void SetQuestGiverState(Scene scene, LoadSceneMode loadSceneMode)
//{
//    foreach (string uid in QuestItemsPool.Keys)
//    {
//        foreach (QuestItem questItem in QuestItemsPool[uid])
//        {
//            if (questItem.wasCollected)
//            {
//                foreach (QuestGiver saveableEntity in FindObjectsOfType<QuestGiver>())
//                {
//                    if (uid != saveableEntity.id) continue;

//                    saveableEntity.OnQuestItemCollected();
//                }
//            }
//        }
//    }
//}