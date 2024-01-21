using UnityEngine;
using System;
using TMPro;

namespace RPG.Quest
{
    public class AnswerButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI answerText;
        public int answerIndex;
        public Action<int> OnAnswerChosen;

        public void SetanswerIndex(int answerIndex) => this.answerIndex = answerIndex;

        public void SetAnswerText(string answer) => answerText.text = answer;

        public void OnClick() => OnAnswerChosen?.Invoke(answerIndex);
    }
}
