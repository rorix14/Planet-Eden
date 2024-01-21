using UnityEngine;
using TMPro;

namespace RPG.Quest
{
    public class InteractIndicator : MonoBehaviour
    {
        [SerializeField] private LeanTweenType easing;
        [SerializeField] private float distanceTraveled = 2f;
        [SerializeField] private float easingTime = 0.5f;
        [SerializeField] private TextMeshProUGUI icon;

        public Color Color { set => icon.GetComponent<TextMeshProUGUI>().color = value; }

        private void Awake() => LeanTween.moveLocalY(gameObject, distanceTraveled, easingTime).setLoopPingPong().setEase(easing);
    }
}
