using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace RPG.Quest
{
    public class QuestLog : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI title = null;
        [SerializeField] private TextMeshProUGUI description = null;
        [SerializeField] private TextMeshProUGUI status = null;
        [SerializeField] private Image statusIcon = null;
        [SerializeField] private Image border = null;

        public TextMeshProUGUI Title => title;
        public TextMeshProUGUI Description => description;
        public TextMeshProUGUI Status => status;
        public Image StatusIcon => statusIcon;
        public Image Border => border;
    }
}