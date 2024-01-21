using UnityEngine;
using TMPro;
using RPG.Stats;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] GameObject character = null;
        [SerializeField] RectTransform foregreound = null;
        [SerializeField] TextMeshProUGUI levelText = null;
        [SerializeField] Canvas rootCanvas;
        private Health health;

        private void Awake()
        {
            rootCanvas.enabled = false;
            health = character.GetComponent<Health>();
        }

        private void Start() => levelText.text = string.Format("L: {0:0}", character.GetComponent<BaseStats>().Level);

        void Update()
        {
            if (Mathf.Approximately(health.HealthPercentage / 100, 0) || Mathf.Approximately(health.HealthPercentage / 100, 1))
            {
                rootCanvas.enabled = false;
                return;
            }
  
            float vel = -2.2f;
            foregreound.localScale =
                new Vector3(Mathf.SmoothDamp(foregreound.localScale.x, health.HealthPercentage / 100, ref vel, 0.1f), 1, 1);
        }

        public void OnHealthChange() => rootCanvas.enabled = true;

        private void OnEnable() => health.OnHealthChanged += OnHealthChange;

        private void OnDisable() => health.OnHealthChanged -= OnHealthChange;
    }
}