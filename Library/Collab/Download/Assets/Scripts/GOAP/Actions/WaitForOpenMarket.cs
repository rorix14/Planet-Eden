using UnityEngine;

public class WaitForOpenMarket : GAction
{
    public override bool PrePerform()
    {
       GameObject currentWaintingSpot = inventory.FindItemWithTag(PlaceOfInterestType.MarketWaitingArea);
        target = currentWaintingSpot ? 
            currentWaintingSpot : GWorld.Instance.GetQueue(PlaceOfInterestType.MarketWaitingArea).RemoveResourse();

        if (!target) return false;

        if (!currentWaintingSpot)
        {
            GWorld.Instance.GetWorld().ModifyState(States.freeMarketWaintingArea, -1);
            inventory.AddItem(target);
        }

        GWorld.Instance.GetQueue(PlaceOfInterestType.queueToEnterMarket).AddResourse(gameObject);
        return true;
    }

    public override bool PostPerform()
    {
        return true;
    }
}
