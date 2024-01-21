using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Utils
{
    public static class ExtencionMethods
    {
        public static RaycastHit[] SortByDistance(this RaycastHit[] raycastHits)
        {
            float[] distances = new float[raycastHits.Length];

            for (int i = 0; i < raycastHits.Length; i++)
            {
                distances[i] = raycastHits[i].distance;
            }
            Array.Sort(distances, raycastHits);

            return raycastHits;
        }

        public static void RemoveItem<T>(this Queue<T> queue, T itemToRemove)
        {
            var cycleAmount = queue.Count;

            for (int i = 0; i < cycleAmount; i++)
            {
                T item = queue.Dequeue();
                if (item.Equals(itemToRemove)) continue;

                queue.Enqueue(item);
            }
        }

        public static Transform FindChildByRecursion(this Transform aParent, string aName)
        {
            if (aParent == null) return null;

            Transform result = aParent.Find(aName);
            if (result != null) return result;

            foreach (Transform child in aParent)
            {
                result = child.FindChildByRecursion(aName);
                if (result != null) return result;
            }
            return null;
        }

        public static void SetChildrenActivation(this Transform parent, bool activate)
        {
            for (int i = 0; i < parent.childCount; i++) parent.GetChild(i).gameObject.SetActive(activate);
        }

        public static bool IsBetweenRange(this float thisValue, float value1, float value2)
        {
            return thisValue >= Mathf.Min(value1, value2) && thisValue <= Mathf.Max(value1, value2);
        }
    }
}