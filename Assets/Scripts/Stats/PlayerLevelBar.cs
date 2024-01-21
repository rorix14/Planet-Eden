using TMPro;
using UnityEngine;

namespace RPG.Stats
{
    public class PlayerLevelBar : MonoBehaviour
    {
        [SerializeField] private GameObject player = null;
        [SerializeField] private TextMeshProUGUI XPText = null;
        [SerializeField] private TextMeshProUGUI LevelText = null;
        [SerializeField] private RectTransform foregreound = null;
        [SerializeField] private GameObject conteiner;
        [SerializeField] private float easingDuration = 0.5f;
        private float easingVelocity = 0f;
        private BaseStats baseStats = null;
        private Experience experience = null;
        private float percetage;

        private void Awake()
        {
            baseStats = player.GetComponent<BaseStats>();
            experience = player.GetComponent<Experience>();
        }

        private void Start() => UpdateLevelText();

        void Update()
        {
            foregreound.localScale =
           new Vector3(Mathf.SmoothDamp(foregreound.localScale.x, percetage, ref easingVelocity, easingDuration), 1, 1);
        }

        private void UpdateLevelText()
        {
            if (TotalXP(GetLastLevel()) == 0)
            {
                XPText.text = string.Format("Max Level");
                percetage = 1;
            }
            else
            {
                XPText.text = string.Format("{0:0}/{1:0}", GetCurrentXP(GetLastLevel()), TotalXP(GetLastLevel()));
                percetage = GetCurrentXP(GetLastLevel()) / TotalXP(GetLastLevel());
            }

            LevelText.text = string.Format("Level {0:0}", baseStats.Level);
        }

        private float GetLastLevel() => (baseStats.Level == 1) ?
                0f : baseStats.Progression.GetStat(Stat.ExperienceToLevelUp, CharacterClass.Player, baseStats.Level - 1);

        private float TotalXP(float lastLevel) =>
            baseStats.Progression.GetStat(Stat.ExperienceToLevelUp, CharacterClass.Player, baseStats.Level) - lastLevel;

        private float GetCurrentXP(float lastLevel) => experience.ExperiencePoints - lastLevel;

       private void OnEnable() => experience.onExperienceUpdated += UpdateLevelText;

        private void OnDisable() => experience.onExperienceUpdated -= UpdateLevelText;
    }
}