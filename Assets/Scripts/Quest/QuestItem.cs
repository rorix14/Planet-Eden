using System;
using UnityEngine;
using UnityEngine.AI;
using RPG.Control;
using RPG.Movement;
using RPG.SceneManagement;
using RPG.Core;

namespace RPG.Quest
{
    public class QuestItem : MonoBehaviour, IRaycastable, IAction
    {
        [SerializeField] private QuestDialog quest;
        [SerializeField] private Vector3 spawnPlace;
        [SerializeField] private RPGScenes sceneToSpwan;
        [SerializeField] private QuestItemState itemState;
        [SerializeField] private GameObject newQuestIndicator = null;
        [SerializeField] private GameObject[] disableOnCollect = null;
        private Reward[] chosenRewad = null;
        private NavMeshObstacle objNavMesh;
        private Mover playerMover = null;
        public event Action<QuestItem> OnCollected;

        public QuestDialog Quest => quest;
        public Vector3 SpawnPlace => spawnPlace;
        public RPGScenes SceneToSpwan => sceneToSpwan;
        public QuestItemState ItemState { get => itemState; set => itemState = value; }
        public Reward[] ChosenRewad { get => chosenRewad; set => chosenRewad = value; }

        public CursorType GetCursorType() => CursorType.Pickup;

        private void Awake() => objNavMesh = GetComponentInChildren<NavMeshObstacle>();

        public void EnableProperties(bool enable)
        {
            GetComponent<CapsuleCollider>().enabled = enable;
            foreach (GameObject child in disableOnCollect) child.gameObject.SetActive(enable);

            if (enable) objNavMesh.enabled = false;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            playerMover = callingController.GetComponent<Mover>();

            if (!playerMover.CanMoveTo(transform.position) || itemState == QuestItemState.NOT_ACTIVE) return false;

            if (Input.GetMouseButtonDown(0))
            {
                callingController.GetComponent<ActionScheduler>().StarAction(this);
                playerMover.MoveTo(transform.position, 0.7f);
            }

            return true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag != "Player" || itemState == QuestItemState.NOT_ACTIVE) return;

            itemState = QuestItemState.WAS_COLLECTED;
            OnCollected?.Invoke(this);
            EnableProperties(false);
        }

        public void Cancel()
        {
            playerMover?.Cancel();
            playerMover = null;
        }
    }
}