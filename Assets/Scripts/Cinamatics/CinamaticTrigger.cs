using UnityEngine;
using UnityEngine.Playables;
using RPG.Saving;
using RPG.Utils;

namespace RPG.Cinamatics
{
    public class CinamaticTrigger : MonoBehaviour, ISaveable
    {
        LazyValue<bool> hasPalyed;

        private void Awake() => hasPalyed = new LazyValue<bool>(ResetCinematic);

        // should be false, but is true for testing purosess
        private bool ResetCinematic() => false;

        private void Start() => hasPalyed.ForceInit();

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player" || hasPalyed.value) return;

            hasPalyed.value = true;
            GetComponent<PlayableDirector>().Play();
        }

        public object CaptureState() => hasPalyed.value;

        public void RestoreState(object state) => hasPalyed.value = (bool)state;
    }
}
