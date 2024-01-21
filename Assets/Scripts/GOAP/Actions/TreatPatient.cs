using UnityEngine;

namespace RPG.GOAP
{
    public class TreatPatient : GAction
    {
        private GameObject patient = null;

        public override bool PrePerform()
        {
            patient = GWorld.Instance.GetQueue(PlaceOfInterestType.sickPatient).RemoveResourse();

            target = patient?.GetComponent<GAgent>().Inventory.FindItemWithTag(PlaceOfInterestType.sickBed);
            if (!target) return false;

            GWorld.Instance.GetWorld().ModifyState(States.sickPatients, -1);
            return true;
        }

        public override bool PostPerform()
        {
            patient.GetComponent<GAgent>().Beliefs.RemoveState(States.isSick);
            patient = null;
            return true;
        }
    }
}

