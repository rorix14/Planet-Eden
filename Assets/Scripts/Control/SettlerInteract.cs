using System;
using UnityEngine;
using TMPro;
using RPG.Movement;
using RPG.GOAP;
using RPG.Core;

namespace RPG.Control
{
    public class SettlerInteract : MonoBehaviour, IInteract, IAction
    {
        [SerializeField] private float distanceFromPlayer = 2f;
        [SerializeField] private float palyerSpeedFraction = 0.6f;
        private bool isInteracting;
        private GAgent gAgent = null;
        private Transform player = null;
        private Mover playerMover = null;
        public event Action<bool> removeControl;

        private void Awake() => gAgent = GetComponent<GAgent>();

        private void Update()
        {
            if (!player) return;

            if (playerMover.TargetIsAtDestination() && !isInteracting)
            {
                transform.LookAt(player.transform);
                player.transform.LookAt(transform);

                gAgent.DialogConteiner.SetActive(true);
                gAgent.DialogConteiner.GetComponentInChildren<TextMeshProUGUI>().text = gAgent.CurrentAction.ActionDialog;

                isInteracting = true;
            }
        }

        public void Interact(GameObject player)
        {
            this.player = player.transform;
            playerMover = player.GetComponent<Mover>();
            Vector3 targetPos = transform.position - Vector3.Normalize(transform.position - this.player.position) * distanceFromPlayer;
            playerMover.MoveTo(targetPos, palyerSpeedFraction);
            gAgent.GAnimation.NavAgent.isStopped = true;
            isInteracting = false;
            player.GetComponent<ActionScheduler>().StarAction(this);
        }

        public void Cancel()
        {
            playerMover.Cancel();
            player = null;
            playerMover = null;
            gAgent.GAnimation.NavAgent.isStopped = false;
            gAgent.DialogConteiner.SetActive(false);
        }
    }
}