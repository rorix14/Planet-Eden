using UnityEngine;
using RPG.Utils;
using RPG.Core;

namespace RPG.Quest
{
    public class QuestGiverDialog
    {
        private QuestGiver questGiver;
        private LazyValue<DialogCanvas> dialogCanvas;
        private CameraMovement cameraMovement = null;

        public DialogCanvas DialogCanvas => dialogCanvas.value;

        public QuestGiverDialog(GameObject questGiver)
        {
            this.questGiver = questGiver.GetComponent<QuestGiver>();
            dialogCanvas = new LazyValue<DialogCanvas>(() => Object.FindObjectOfType<DialogCanvas>());
            dialogCanvas.ForceInit();
            cameraMovement = Object.FindObjectOfType<CameraMovement>();
        }

        public void StartDialog()
        {
            questGiver.GetFirstQuestGiverLine();

            foreach (AnswerButton button in dialogCanvas.value.AnswerButtonsPool) button.OnAnswerChosen = questGiver.UpdateState;

            dialogCanvas.value.ShowDialogCanvas();
            dialogCanvas.value.SetNpcDialog(questGiver.CurrentLine.text);
            ManageAnswerButtons();
            cameraMovement.UIElementStatus(this ,true);
        }

        public void ManageAnswerButtons()
        {
            if (questGiver.CurrentLine.answers.Length > dialogCanvas.value.AnswerButtonsPool.Count)
            {
                for (int i = dialogCanvas.value.AnswerButtonsPool.Count; i < questGiver.CurrentLine.answers.Length; i++)
                {
                    AnswerButton button = Object.Instantiate(dialogCanvas.value.AnswerBtnPrefab, dialogCanvas.value.AnswerContent);
                    button.SetanswerIndex(i);
                    dialogCanvas.value.AnswerButtonsPool.Add(button);
                    dialogCanvas.value.AnswerButtonsPool[i].OnAnswerChosen = questGiver.UpdateState;
                }
            }
            else
            {
                for (int i = questGiver.CurrentLine.answers.Length; i < dialogCanvas.value.AnswerButtonsPool.Count; i++)
                {
                    dialogCanvas.value.AnswerButtonsPool[i].gameObject.SetActive(false);
                }
            }

            for (int i = 0; i < questGiver.CurrentLine.answers.Length; i++)
            {
                dialogCanvas.value.AnswerButtonsPool[i].gameObject.SetActive(true);
                dialogCanvas.value.AnswerButtonsPool[i].SetAnswerText(questGiver.CurrentLine.answers[i].text);
            }
        }

        public void CloseDialog()
        {
            dialogCanvas.value.CloseDialogCanvas();
            cameraMovement.UIElementStatus(this ,false);
        }
    }
}