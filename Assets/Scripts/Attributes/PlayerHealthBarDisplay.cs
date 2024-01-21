using UnityEngine;
using TMPro;
namespace RPG.Attributes
{
    public class PlayerHealthBarDisplay : MonoBehaviour
    {
        [SerializeField] private Health health = null;
        [SerializeField] private TextMeshProUGUI healthText = null;
        [SerializeField] private RectTransform foregreound = null;
        [SerializeField] private GameObject conteiner;
        [SerializeField] private float easingDuration = 0.15f;
        private float easingVelocity = 0f;

        private void Start() => OnHealthChange();
      
        void Update()
        {          
            foregreound.localScale = 
            new Vector3(Mathf.SmoothDamp(foregreound.localScale.x, health.HealthPercentage / 100, ref easingVelocity, easingDuration), 1, 1);
        }

        public void OnHealthChange() => healthText.text = string.Format("{0:0}/{1:0}", health.HealthPoints, health.MaxHealthPoints);

        private void OnEnable() => health.OnHealthChanged += OnHealthChange;

        private void OnDisable() => health.OnHealthChanged -= OnHealthChange;
    }
}
