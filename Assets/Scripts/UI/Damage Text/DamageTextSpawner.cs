using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] DamageText damageText = null;

        public void Spawn(float damage)
        {
            DamageText instance = Instantiate(damageText, transform);
            instance.Setvalue(damage);
        }
    }
}
