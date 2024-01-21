using UnityEngine;

public class WaterPlant : GAction
{
    public override bool PrePerform()
    {
        target = GWorld.Instance.GetQueue(PlaceOfInterestType.Plant).RemoveResourse();
        if (!target) return false;
        GWorld.Instance.GetWorld().ModifyState(States.plantToWater, -1);
        return true;
    }

    public override bool PostPerform()
    {
        LeanTween.scale(target.transform.GetChild(0).gameObject, new Vector3(2, 2, 2), 1f).setEaseInElastic();

        GWorld.Instance.GetQueue(PlaceOfInterestType.CollectablePlants).AddResourse(target);
        GWorld.Instance.GetWorld().ModifyState(States.plantColactable, 1);
        return true;
    }
}
