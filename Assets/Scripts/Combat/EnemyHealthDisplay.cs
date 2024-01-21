using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter;
        void Awake() => fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();

        void Update() => GetComponent<Text>().text = !fighter.Target ? "N/A" : string.Format("{0:0}/{1:0}", fighter.Target.HealthPoints, fighter.Target.MaxHealthPoints);
    }
}
