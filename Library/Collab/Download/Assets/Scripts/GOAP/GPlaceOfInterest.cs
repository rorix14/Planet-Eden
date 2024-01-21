using UnityEngine;

public enum PlaceOfInterestType
{
    Lounge,
    patients,
    Cubicle,
    Office,
    Toilet,
    puddles,
    //new ones
    player,
    Plant,
    Market,
    CollectablePlants,
    WaintingArea,
    WaitingCollectors,
    MarketWaitingArea,
    queueToEnterMarket
}

public enum States
{  
    patients,
    FreeCubicle,
    FreeOffice,
    FreeToilet,
    FreePuddle,
    TreatingPatient,
    //for count purpuses
    Treated,
    Waiting,
    //goals
    clean,
    rested,
    research,
    toilet,
    treatPatient,
    isWaiting,
    isTreated,
    isHome,
    //personalStates
    exhausted,
    relief,
    isCured,
    atHospital,
    // new ones
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
    // new states
    playerInteract,
    isInteracted
}

public class GPlaceOfInterest : MonoBehaviour
{
    [SerializeField] private PlaceOfInterestType resourceType;
    [SerializeField] private States state;

    public PlaceOfInterestType ResorceType => resourceType;
    public States State => state;
}
