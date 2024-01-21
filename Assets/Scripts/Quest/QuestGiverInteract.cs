using RPG.Movement;
using UnityEngine;
using System;
using RPG.Control;
using RPG.Core;

namespace RPG.Quest
{
    public class QuestGiverInteract : MonoBehaviour, IInteract, IAction
    {
        [SerializeField] private float distanceFromPlayer = 2f;
        [Range(0, 1)]
        [SerializeField] private float palyerSpeedFraction = 0.6f;
        private Transform player = null;
        private Mover playerMover = null;
        private bool isInteracting;
        private QuestGiverDialog questGiverDialog;
        private Animator animator = null;
        private string affiliationAniation = "Neutral";
        public QuestGiverDialog QuestGiverDialog => questGiverDialog;

        public event Action<bool> removeControl;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            questGiverDialog = new QuestGiverDialog(gameObject);
        }

        void Update()
        {
            if (!player) return;

            if (playerMover.TargetIsAtDestination() && !isInteracting)
            {
                transform.LookAt(player.transform);
                player.transform.LookAt(transform);
                removeControl?.Invoke(false);

                questGiverDialog.StartDialog();

                isInteracting = true;
            }
        }

        public void SetAffiliationAniation(int affiliationPoints)
        {
            AffilationSatus affilationSatus = AffilationSatus.NEUTRAL;
            float minvalue = Mathf.Infinity;

            foreach (AffilationSatus affilation in Enum.GetValues(typeof(AffilationSatus)))
            {
                float dif = Mathf.Abs(affiliationPoints - (int)affilation);
                if (dif < minvalue)
                {
                    minvalue = dif;
                    affilationSatus = affilation;
                }
            }

            switch (affilationSatus)
            {
                case AffilationSatus.NEUTRAL:
                    affiliationAniation = "Neutral";
                    break;
                case AffilationSatus.POSITIVE:
                    affiliationAniation = "Positive";
                    break;
                case AffilationSatus.NEGATIVE:
                    affiliationAniation = "Negative";
                    break;
            }
        }

        public void Interact(GameObject player)
        {
            this.player = player.transform;
            playerMover = player.GetComponent<Mover>();
            Vector3 targetPos = transform.position - Vector3.Normalize(transform.position - this.player.position) * distanceFromPlayer;
            playerMover.MoveTo(targetPos, palyerSpeedFraction);
            player.GetComponent<ActionScheduler>().StarAction(this);
        }

        public void Cancel()
        {
            playerMover?.Cancel();
            player = null;
            playerMover = null;
            removeControl?.Invoke(true);
            questGiverDialog.CloseDialog();

            if (isInteracting) animator.SetTrigger(affiliationAniation);
            isInteracting = false;
        }
    }
}