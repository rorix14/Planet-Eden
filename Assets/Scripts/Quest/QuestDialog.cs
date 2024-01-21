using System.Collections.Generic;
using System;
using UnityEngine;

// change name to quest data
namespace RPG.Quest
{
    [CreateAssetMenu(fileName = "Quest Giver", menuName = "New Quest Giver", order = 0)]
    public class QuestDialog : ScriptableObject
    {
        [SerializeField] private string questTitle;
        [TextArea(2, 3)]
        [SerializeField] private string questDescription;
        [SerializeField] private ItemStateDescription[] ItemStateDescription;
        [SerializeField] private QuestReward[] questRewards;
        [SerializeField] private StateLines[] stateLines;
        private int nextLineIndex;
        private Dictionary<QuestItemState, string> itemStateToDiscreptionLookUp;
        private Dictionary<QuestDialogState, Lines[]> stateLinesLookUp;

        public string QuestTitle => questTitle;
        public string QuestDescription => questDescription;

        public string GetStatusDescreption(QuestItemState itemState)
        {
            if (itemStateToDiscreptionLookUp == null) BuildItemStateToDiscreptionLookUp();

            return itemStateToDiscreptionLookUp.ContainsKey(itemState) ? itemStateToDiscreptionLookUp[itemState] : "";
        }

        public AffilationLine FirstLine(QuestDialogState currentState, int affilationValue)
        {
            Lines[] currentStateLines = GetStateLines(currentState);

            nextLineIndex = 0;
            return GetAffilationLine(currentStateLines[nextLineIndex].affilationLines, affilationValue);
        }

        public AffilationLine NextLine(ref QuestDialogState currentState, ref int totalAffiliationPoints, Action OnQuestActivated, Action<Reward[]> OnItemDelivered)
        {
            Lines[] currentStateLines = GetStateLines(currentState);
            AffilationLine nextLine = GetAffilationLine(currentStateLines[nextLineIndex].affilationLines, totalAffiliationPoints);

            if (nextLine.answerIndex != -1)
            {
                nextLineIndex = nextLine.answers[nextLine.answerIndex].lineIndex;
                totalAffiliationPoints += nextLine.answers[nextLine.answerIndex].affiliationValue;

                if (nextLineIndex == -1)
                {
                    nextLineIndex = 0;

                    switch (currentState)
                    {
                        case QuestDialogState.INITIAL:
                            OnQuestActivated?.Invoke();
                            currentState = QuestDialogState.WAITING;
                            break;
                        case QuestDialogState.COMPLEATED:
                            OnItemDelivered?.Invoke(GetReward(totalAffiliationPoints));
                            currentState = QuestDialogState.POSTCOMPLETION;
                            break;
                    }

                    return null;
                }
            }

            return GetAffilationLine(currentStateLines[nextLineIndex].affilationLines, totalAffiliationPoints);
        }

        private Lines[] GetStateLines(QuestDialogState state)
        {
            if (stateLinesLookUp == null) BuildStateLinesLookUpLookUp();

            return stateLinesLookUp.ContainsKey(state) ? stateLinesLookUp[state] : stateLines[0].lines;
        }

        private AffilationLine GetAffilationLine(AffilationLine[] affilationLines, int affilationValue)
        {
            int index = 0;
            int rewardIndex = 0;
            float minvalue = Mathf.Infinity;
            foreach (AffilationLine affilationLine in affilationLines)
            {
                ClossestAffiliation(affilationValue, index, ref rewardIndex, ref minvalue, affilationLine.affilation);
                index++;
            }

            return affilationLines[rewardIndex];
        }


        private Reward[] GetReward(int affiliationPoints)
        {
            int index = 0;
            int rewardIndex = 0;
            float minvalue = Mathf.Infinity;
            foreach (QuestReward questReward in questRewards)
            {
                ClossestAffiliation(affiliationPoints, index, ref rewardIndex, ref minvalue, questReward.affilationRequired);
                index++;
            }

            return questRewards[rewardIndex].rewards;
        }

        private void ClossestAffiliation(int affilationValue, int index, ref int closestIndex, ref float minvalue, AffilationSatus affilation)
        {
            float dif = Mathf.Abs(affilationValue - (int)affilation);
            if (dif < minvalue)
            {
                minvalue = dif;
                closestIndex = index;
            }
        }

        private void BuildItemStateToDiscreptionLookUp()
        {
            itemStateToDiscreptionLookUp = new Dictionary<QuestItemState, string>();

            foreach (ItemStateDescription item in ItemStateDescription) itemStateToDiscreptionLookUp[item.itemState] = item.line;
        }

        private void BuildStateLinesLookUpLookUp()
        {
            stateLinesLookUp = new Dictionary<QuestDialogState, Lines[]>();
            foreach (StateLines state in stateLines) stateLinesLookUp[state.state] = state.lines;
        }
    }
}