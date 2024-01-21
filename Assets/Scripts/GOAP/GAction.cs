using System.Collections.Generic;
using UnityEngine;
using System;

namespace RPG.GOAP
{
    public abstract class GAction : MonoBehaviour
    {
        [Serializable]
        public class WorldState
        {
            public States key;
            public int value;
        }

        [SerializeField] public PlaceOfInterestType targetTag;
        [SerializeField] private string actionDialog = "";
        [SerializeField] private float cost = 1f;
        [SerializeField] protected float actionVelocitty = 5f;
        [SerializeField] private float actionDuration = 0f;
        [SerializeField] private float rotateToTargetSpeed = 0f;
        [SerializeField] private string [] animatorTrigger;
        [SerializeField] private WorldState[] preConditions;
        [SerializeField] private WorldState[] afterEffects;
        [SerializeField] private bool runnning = false;
        protected GameObject target = null;
        protected WorldStates agentBeliefs = null;
        protected GInventory inventory = null;
        private Dictionary<States, int> conditions = new Dictionary<States, int>();
        private Dictionary<States, int> effects = new Dictionary<States, int>();

        public string ActionDialog => actionDialog;
        public float Cost => cost;
        public float ActionVelocitty => actionVelocitty;
        public float ActionDuration => actionDuration;
        public GameObject Target { get => target; set => target = value; }
        public bool Runnning { get => runnning; set => runnning = value; }
        public Dictionary<States, int> Effects => effects;
        public Dictionary<States, int> Conditions => conditions;
        public string[] AnimatorTrigger => animatorTrigger;
        public float RotateToTargetSpeed => rotateToTargetSpeed;

        private void Awake()
        {
            inventory = GetComponent<GAgent>().Inventory;
            agentBeliefs = GetComponent<GAgent>().Beliefs;

            BuildDictionary(preConditions, conditions);
            BuildDictionary(afterEffects, effects);
        }

        public bool IsAchivable() => true;

        public bool IsAchivableGiven(Dictionary<States, int> conditions)
        {
            foreach (KeyValuePair<States, int> item in this.conditions) if (!conditions.ContainsKey(item.Key)) return false;

            return true;
        }

        public abstract bool PrePerform();
        public abstract bool PostPerform();

        private void BuildDictionary(WorldState[] worldStates, Dictionary<States, int> dic)
        {
            if (worldStates != null) foreach (WorldState preCondition in worldStates) dic.Add(preCondition.key, preCondition.value);
        }
    }
}