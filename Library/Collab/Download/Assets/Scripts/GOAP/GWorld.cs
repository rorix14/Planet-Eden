using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using RPG.Utils;

public class ResourceQueue
{
    private Queue<GameObject> que = new Queue<GameObject>();
    private PlaceOfInterestType placeOfInterest;
    private States modState;

    public Queue<GameObject> Que => que;

    public ResourceQueue(WorldStates world, PlaceOfInterestType placeOfInterest)
    {
        this.placeOfInterest = placeOfInterest;

        if (this.placeOfInterest != PlaceOfInterestType.patients)
        {
            GPlaceOfInterest[] resourses = UnityEngine.Object.FindObjectsOfType<GPlaceOfInterest>();
            foreach (GPlaceOfInterest resourse in resourses)
            {
                if (resourse.ResorceType != this.placeOfInterest) continue;
                modState = resourse.State;
                que.Enqueue(resourse.gameObject);
            }
        }

        if (modState != States.patients) world.ModifyState(modState, que.Count);
    }

    public void AddResourse(GameObject resourse) => que.Enqueue(resourse);

    public GameObject RemoveResourse()
    {
        if (que.Count == 0) return null;
        return que.Dequeue();
    }

    public void RemoveSpecificResource(GameObject objToRemove) => que.RemoveItem(objToRemove);

    public bool HasResource() => que.Count != 0;
}

public sealed class GWorld
{
    private static readonly GWorld instance = new GWorld();
    private WorldStates world;
    private Dictionary<PlaceOfInterestType, ResourceQueue> resourses;

    private GWorld() { }
   
    static GWorld()
    {
        instance.InitializeGworld();
        SceneManager.sceneLoaded += (arg0, arg1) => Instance.InitializeGworld();
    }

    private void InitializeGworld()
    {
        world = new WorldStates();
        resourses = new Dictionary<PlaceOfInterestType, ResourceQueue>();

        foreach (PlaceOfInterestType placeOfInterest in (PlaceOfInterestType[])Enum.GetValues(typeof(PlaceOfInterestType)))
        {
            ResourceQueue resourceQueue = new ResourceQueue(world, placeOfInterest);
            resourses.Add(placeOfInterest, resourceQueue);
        }
    }

    public static GWorld Instance => instance;

    public ResourceQueue GetQueue(PlaceOfInterestType type) => resourses[type];

    public WorldStates GetWorld() => world;
}