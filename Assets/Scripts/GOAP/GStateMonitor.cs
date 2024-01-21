using System.Collections;
using UnityEngine;
using TMPro;

namespace RPG.GOAP
{
    public abstract class GStateMonitor : MonoBehaviour
    {
        [SerializeField] protected GAction action;
        [SerializeField] protected States state;
        [SerializeField] private string situationalLine;
        [SerializeField] protected float stateStrength, stateChangeRate;
        [SerializeField] private float dialogTimeOnScreen = 1.5f;
        protected WorldStates beliefs;
        protected bool stateFound = false;
        protected float intialStenght;
        private TextMeshProUGUI dilagUI;
        private GAgent agent = null;

        private void Awake()
        {
            agent = GetComponent<GAgent>();
            beliefs = agent.Beliefs;
            dilagUI = agent.DialogConteiner.GetComponentInChildren<TextMeshProUGUI>();
            intialStenght = stateStrength;
        }

        protected IEnumerator Sayline()
        {
            agent.DialogConteiner.SetActive(true);
            dilagUI.text = situationalLine;
            yield return new WaitForSeconds(dialogTimeOnScreen);
            agent.DialogConteiner.SetActive(false);
        }
    }
}