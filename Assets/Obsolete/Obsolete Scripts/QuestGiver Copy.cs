using System;
using RPG.Movement;
using RPG.Saving;
using RPG.Utils;
using UnityEngine;

namespace RPG.Quest
{/* REMOVE HERE
    public class QuestGiverCopy : MonoBehaviour, ISaveable
    {
        [SerializeField] public QuestItem[] questItems;
        [SerializeField] public int currentItemIndex;
        [SerializeField] public QuestState currentState;

        private QuestGiverInteract questGiverInteract;
        private QuestGiverDialog questGiverDialog;

        // change properties to private / make setters getter if needed
        public Line currentLine = null;
        public string id;
        public QuestsManager questsManager;
        public bool questCompleated;
        public static event Action OnquestCompleated;

        // [SerializeField] private float distanceFromPlayer = 2f;
        // [Range (0, 1)]
        // [SerializeField] private float palyerSpeedFraction = 0.6f;
        // private Transform player = null;
        // private Mover playerMover = null;
        // private bool isInteracting;
        // public event Action<bool> removeControl;

        public string Id => id;

        private void Awake()
        {
            // dialogCanvas = new LazyValue<DialogCanvas>(DialogCanvasInit);
            questGiverDialog = GetComponent<QuestGiverDialog>();
            questsManager = FindObjectOfType<QuestsManager>();
            id = GetComponent<SaveableEntity>().UniqueIdentifier;
            questGiverInteract = GetComponent<QuestGiverInteract>();
        }

        private void Start()
        {
            //dialogCanvas.ForceInit();
            questsManager.AddQuestItem(id, questItems, currentState);
        }

        // private void Update () {
        //     if (!player) return;

        //     if (playerMover.TargetIsAtDestination () && !isInteracting) {
        //         transform.LookAt (player.transform);
        //         player.transform.LookAt (transform);
        //         removeControl?.Invoke (false);

        //         foreach (AnswerButton button in dialogCanvas.value.AnswerButtonsPool) button.OnAnswerChosen = SetDialogAnswer;

        //         dialogCanvas.value.Canvas.SetActive (true);
        //         currentLine = GetCurrentQuest ().FirstLine (currentState);
        //         dialogCanvas.value.SetNpcDialog (currentLine.text);
        //         ManageAnswerButtons ();
        //         isInteracting = true;
        //     }
        // }

        // public void Interact (GameObject player) {
        //     this.player = player.transform;
        //     playerMover = player.GetComponent<Mover> ();
        //     Vector3 targetPos = transform.position - Vector3.Normalize (transform.position - this.player.position) * distanceFromPlayer;
        //     player.GetComponent<Mover> ().MoveTo (targetPos, palyerSpeedFraction);
        // }

        // public void CancelInteraction () {
        //     player = null;
        //     removeControl?.Invoke (true);
        //     dialogCanvas.value.Canvas.SetActive (false);
        //     isInteracting = false;
        // }

        public void UpdateState(int answerIndex)
        {
            currentLine.answerIndex = answerIndex;
            int lastStateIndex = (int)currentState;

            currentLine = GetNextQuestGiverLine();

            if (currentLine == null)
            {
                if (lastStateIndex == (int)QuestState.POSTCOMPLETION)
                {
                    if (currentItemIndex != questItems.Length - 1)
                    {
                        currentItemIndex++;
                        currentState = QuestState.INITIAL;
                    }
                    else
                    {
                        questCompleated = true;
                        OnquestCompleated?.Invoke();
                    }
                }

                questsManager.ChangeQuestGiverState(id, currentState);
                questGiverInteract.CancelInteraction();
                return;
            }

            questGiverDialog.dialogCanvas.value.SetNpcDialog(currentLine.text);
            questGiverDialog.ManageAnswerButtons();
        }

        public Line GetNextQuestGiverLine()
        {
            return GetCurrentQuest().NextLine(ref currentState);
        }

        // private void ManageAnswerButtons () {
        //     if (currentLine.answers.Length > dialogCanvas.value.AnswerButtonsPool.Count) {
        //         for (int i = dialogCanvas.value.AnswerButtonsPool.Count; i < currentLine.answers.Length; i++) {
        //             AnswerButton button = Instantiate (dialogCanvas.value.AnswerBtnPrefab, dialogCanvas.value.Panel);
        //             button.SetanswerIndex (i);
        //             dialogCanvas.value.AnswerButtonsPool.Add (button);
        //             dialogCanvas.value.AnswerButtonsPool[i].OnAnswerChosen = SetDialogAnswer;
        //         }
        //     } else {
        //         for (int i = currentLine.answers.Length; i < dialogCanvas.value.AnswerButtonsPool.Count; i++) {
        //             dialogCanvas.value.AnswerButtonsPool[i].gameObject.SetActive (false);
        //         }
        //     }

        //     for (int i = 0; i < currentLine.answers.Length; i++) {
        //         dialogCanvas.value.AnswerButtonsPool[i].gameObject.SetActive (true);
        //         dialogCanvas.value.AnswerButtonsPool[i].SetAnswerText (currentLine.answers[i].text);
        //     }
        // }

        // private DialogCanvas DialogCanvasInit () => FindObjectOfType<DialogCanvas> ();

        private QuestItem GetCurrentItem() => questItems[currentItemIndex];

        public Quest GetCurrentQuest() => GetCurrentItem().Quest;

        private void OnQuestActivated() => questsManager.CanBeCollected(id, currentItemIndex);

        private void OnItemDelivered() => questsManager.DeliveredItem(id, currentItemIndex);

        private void OnEnable()
        {
            foreach (QuestItem questItem in questItems)
            {
                questItem.Quest.OnQuestActivated += OnQuestActivated;
                questItem.Quest.OnItemDelivered += OnItemDelivered;
            }
        }

        private void OnDisable()
        {
            foreach (QuestItem questItem in questItems)
            {
                questItem.Quest.OnQuestActivated -= OnQuestActivated;
                questItem.Quest.OnItemDelivered -= OnItemDelivered;
            }
        }

        [Serializable]
        private struct QuestGiverSavables
        {
            public int currentItemIndex;
            public bool questCompleated;
        }

        public object CaptureState()
        {
            return new QuestGiverSavables()
            {
                currentItemIndex = currentItemIndex,
                questCompleated = questCompleated
            };
        }

        public void RestoreState(object state)
        {
            QuestGiverSavables nPCSavables = (QuestGiverSavables)state;
            currentItemIndex = nPCSavables.currentItemIndex;
            questCompleated = nPCSavables.questCompleated;
        }
    }
    REMOVE HERE */
}