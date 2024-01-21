using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] Text damageText = null;
        public void Setvalue(float amount) => damageText.text = string.Format("{0:0}", amount);

        // Animation event
        public void DestroyText() => Destroy(gameObject);
    }
}
