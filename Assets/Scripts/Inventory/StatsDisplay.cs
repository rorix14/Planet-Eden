using UnityEngine;
using TMPro;

namespace RPG.Inventory
{
    public class StatsDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI damageText;
        [SerializeField] private TextMeshProUGUI range;
        [SerializeField] private TextMeshProUGUI bonusDamage;
        [SerializeField] private TextMeshProUGUI attackRate;
        [SerializeField] private LeanTweenType easing;
        [SerializeField] private float easingTime = 0.2f;

        public TextMeshProUGUI DamageText => damageText;
        public TextMeshProUGUI Range => range;
        public TextMeshProUGUI BonusDamage => bonusDamage;
        public TextMeshProUGUI AttackRate => attackRate;

        //private void Awake() => transform.localScale = Vector3.zero;

        private void OnEnable()
        {
            transform.localScale = Vector3.zero;
            LeanTween.scale(gameObject, new Vector3(1, 1, 1), easingTime).setEase(easing);
        }

        private void OnDisable() => transform.localScale = Vector3.zero;
    }
}
