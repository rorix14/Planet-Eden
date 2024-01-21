using UnityEngine;

namespace RPG.GOAP
{
    public enum PlaceOfInterestType
    {
        DefaultPlace,
        RestArea,
        player,
        Plant,
        Market,
        CollectablePlants,
        WaintingArea,
        WaitingCollectors,
        MarketWaitingArea,
        queueToEnterMarket,
        sickPatient,
        researchArea,
        sickBed,
        ScavengeSpot,
        DanceArea,
    }

    public enum States
    {
        DefaultState,
        Waiting,      
        rested,     
        exhausted,
        plantToWater,
        waterPlant,
        plantColactable,
        hasPlant,
        deliverPlant,
        freeMarketPlace,
        freeRestPlace,
        Wait,
        freeWaintingSpace,
        waintingToColect,
        freeMarketWaintingArea,
        hasPlantBelief,
        Research,
        TreatPatient,
        GetTreated,
        isSick,
        Cough,
        needsToCough,
        Sleep,
        sickPatients,
        freeResearchArea,
        Scavenge,
        freeScavangeSpot,
        Terrified,
        isTerrified,
        Dance,
        mustDance,
        freeDanceArea,
        spotedPlayer,
        GreatPlayer,
    }

    public class GPlaceOfInterest : MonoBehaviour
    {
        [SerializeField] private PlaceOfInterestType resourceType;
        [SerializeField] private States state;

        public PlaceOfInterestType ResorceType => resourceType;
        public States State => state;
    }
}