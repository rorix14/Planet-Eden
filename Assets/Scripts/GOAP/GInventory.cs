using System.Collections.Generic;
using UnityEngine;

namespace RPG.GOAP
{
    // Rename to GAgentInventory
    public class GInventory
    {
        private List<GameObject> items = new List<GameObject>();

        public List<GameObject> Items => items;

        public void AddItem(GameObject item) => items.Add(item);

        public GameObject FindItemWithTag(PlaceOfInterestType type)
        {
            foreach (GameObject item in items)
            {
                GPlaceOfInterest placeOfInterest = item.GetComponent<GPlaceOfInterest>();
                if (placeOfInterest.ResorceType == type) return item;
            }
            return null;
        }

        public void RemoveItem(GameObject item)
        {
            int indexToRemove = -1;
            foreach (GameObject i in items)
            {
                indexToRemove++;

                if (i == item) break;
            }

            if (indexToRemove >= -1) items.RemoveAt(indexToRemove);
        }
    }
}