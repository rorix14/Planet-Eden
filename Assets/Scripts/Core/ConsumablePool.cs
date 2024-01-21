using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class ConsumablePool : MonoBehaviour
    {
        private Dictionary<ItemPool, Queue<GameObject>> comsumablePool = new Dictionary<ItemPool, Queue<GameObject>>();

        public GameObject GetItemFromPool(ItemPool ItemClass, object objClass)
        {
            if (!comsumablePool.ContainsKey(ItemClass)) comsumablePool[ItemClass] = new Queue<GameObject>();
            if (comsumablePool[ItemClass].Count == 0) CreateItem(ItemClass, objClass);

            return comsumablePool[ItemClass].Dequeue();
        }

        public void ReturnToPool(ItemPool ItemClass, GameObject objToreturn)
        {
            objToreturn.transform.parent = null;
            DontDestroyOnLoad(objToreturn);
            AddToQueue(ItemClass, objToreturn);
        }

        private void CreateItem(ItemPool ItemClass, object objClass)
        {
            GameObject newItem = Instantiate(objClass as Component).gameObject;
            AddToQueue(ItemClass, newItem);
        }

        private void AddToQueue(ItemPool ItemClass, GameObject newItem)
        {
            newItem.SetActive(false);
            comsumablePool[ItemClass].Enqueue(newItem);
        }

        // IF DONT DESTROY ON LOAD DOES NOT WORK USE THIS PEACE OF CODE \\

        //private void ResetQueues(Scene scene, LoadSceneMode loadSceneMode)
        //{
        //    foreach (Queue<GameObject> itemQueue in comsumablePool.Values) itemQueue.Clear();
        //}
        //private void OnEnable() => SceneManager.sceneLoaded += ResetQueues;
        //private void OnDisable() => SceneManager.sceneLoaded -= ResetQueues;
    }
}