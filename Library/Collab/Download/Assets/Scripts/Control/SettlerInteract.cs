using System;
using UnityEngine;
using TMPro;
using RPG.Movement;

namespace RPG.Control
{
    public class SettlerInteract : MonoBehaviour, IInteract
    {
        [SerializeField] private float distanceFromPlayer = 2f;
        [SerializeField] private float palyerSpeedFraction = 0.6f;
        [SerializeField] private GameObject dialogConteiner = null;
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

                dialogConteiner.SetActive(true);
                dialogConteiner.GetComponentInChildren<TextMeshProUGUI>().text = gAgent.CurrentAction.ActionDialog;

                isInteracting = true;
            }
        }

        public void CancelInteraction()
        {
            player = null;
            playerMover = null;
            gAgent.GAnimation.NavAgent.isStopped = false;
            dialogConteiner.SetActive(false);
        }

        public void Interact(GameObject player)
        {
            this.player = player.transform;
            playerMover = player.GetComponent<Mover>();
            Vector3 targetPos = transform.position - Vector3.Normalize(transform.position - this.player.position) * distanceFromPlayer;
            player.GetComponent<Mover>().MoveTo(targetPos, palyerSpeedFraction);
            gAgent.GAnimation.NavAgent.isStopped = true;
            isInteracting = false;
        }
    }
}
