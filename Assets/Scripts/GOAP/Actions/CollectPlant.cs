using UnityEngine;

namespace RPG.GOAP
{
    public class CollectPlant : GAction
    {
        public override bool PrePerform()
        {
            ResourceQueue WaitingCollectorsQue = GWorld.Instance.GetQueue(PlaceOfInterestType.WaitingCollectors);

            target = GWorld.Instance.GetQueue(PlaceOfInterestType.CollectablePlants).RemoveResourse();
            if (!target) return false;

            if (WaitingCollectorsQue.HasResource())
            {
                if (!WaitingCollectorsQue.Que.Contains(gameObject) || WaitingCollectorsQue.Que.Peek() != gameObject)
                {
                    GWorld.Instance.GetQueue(PlaceOfInterestType.CollectablePlants).AddResourse(target);
                    return false;
                }
                else WaitingCollectorsQue.RemoveSpecificResource(gameObject);
            }

            GameObject inventoryWaintingArea = inventory.FindItemWithTag(PlaceOfInterestType.WaintingArea);
            if (inventoryWaintingArea)
            {
                inventory.RemoveItem(inventoryWaintingArea);
                GWorld.Instance.GetQueue(PlaceOfInterestType.WaintingArea).AddResourse(inventoryWaintingArea);
                GWorld.Instance.GetWorld().ModifyState(States.freeWaintingSpace, 1);
            }

            GWorld.Instance.GetWorld().ModifyState(States.plantColactable, -1);
            return true;
        }

        public override bool PostPerform()
        {
            LeanTween.scale(target.transform.GetChild(0).gameObject, new Vector3(1, 1, 1), 1f).setEaseInBack();

            GWorld.Instance.GetQueue(PlaceOfInterestType.Plant).AddResourse(target);
            GWorld.Instance.GetWorld().ModifyState(States.plantToWater, 1);

            agentBeliefs.ModifyState(States.hasPlantBelief, 0);
            return true;
        }
    }
}