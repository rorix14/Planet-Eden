using System;
using UnityEngine;
using RPG.Inventory;

namespace RPG.Quest
{
    public enum AffilationSatus
    {
        NEUTRAL = 0,
        POSITIVE = 10,
        NEGATIVE = -10
    }

    [Serializable]
    public class StateLines
    {
        public QuestDialogState state;
        public Lines[] lines;
    }

    [Serializable]
    public class Lines
    {
        public AffilationLine[] affilationLines;
    }

    [Serializable]
    public class AffilationLine
    {
        public AffilationSatus affilation;
        [TextArea(2, 3)]
        public string text;
        public Answer[] answers;
        public int answerIndex = -1;
    }

    [Serializable]
    public class Answer
    {
        public string text;
        public int lineIndex;
        public int affiliationValue = 0;
    }

    [Serializable]
    public class ItemStateDescription
    {
        public QuestItemState itemState;
        public string line;
    }

    [Serializable]
    public class QuestReward
    {
        public AffilationSatus affilationRequired;
        public Reward[] rewards;
    }

    [Serializable]
    public class Reward
    {
        public ConsumableType consumableType;
        public int amount;
    }
}