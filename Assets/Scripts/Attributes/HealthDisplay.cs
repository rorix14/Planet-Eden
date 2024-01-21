using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    // to remove
    public class HealthDisplay : MonoBehaviour
    {
        Health health;
        void Awake() => health = GameObject.FindWithTag("Player").GetComponent<Health>();

        void Update() => GetComponent<Text>().text = string.Format("{0:0}/{1:0}", health.HealthPoints, health.MaxHealthPoints);
    }
}

