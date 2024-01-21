using UnityEngine;

public class GotoMarket : GAction
{
    public override bool PrePerform()
    {
        ResourceQueue WaitingTogoToMakket = GWorld.Instance.GetQueue(PlaceOfInterestType.queueToEnterMarket);

        target = GWorld.Instance.GetQueue(PlaceOfInterestType.Market).RemoveResourse();
        if (!target) return false;

        if (WaitingTogoToMakket.HasResource())
        {
            if (!WaitingTogoToMakket.Que.Contains(gameObject) || WaitingTogoToMakket.Que.Peek() != gameObject)
            {
                GWorld.Instance.GetQueue(PlaceOfInterestType.Market).AddResourse(target);
                return false;
            }
            else WaitingTogoToMakket.RemoveSpecificResource(gameObject);
        }

        GameObject inventoryWaintingArea = inventory.FindItemWithTag(PlaceOfInterestType.MarketWaitingArea);
        if (inventoryWaintingArea)
        {
            inventory.RemoveItem(inventoryWaintingArea);
            GWorld.Instance.GetQueue(PlaceOfInterestType.MarketWaitingArea).AddResourse(inventoryWaintingArea);
            GWorld.Instance.GetWorld().ModifyState(States.freeMarketWaintingArea, 1);
        }

        GWorld.Instance.GetWorld().ModifyState(States.freeMarketPlace, -1);

        return true;
    }

    public override bool PostPerform()
    {
        if (!GWorld.Instance.GetQueue(PlaceOfInterestType.CollectablePlants).HasResource())
            agentBeliefs.ModifyState(States.Wait, 0);

        GWorld.Instance.GetQueue(PlaceOfInterestType.Market).AddResourse(target);
        GWorld.Instance.GetWorld().ModifyState(States.freeMarketPlace, 1);
        agentBeliefs.RemoveState(States.hasPlantBelief);
        return true;
    }
}