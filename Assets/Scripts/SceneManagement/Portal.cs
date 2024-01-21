using UnityEngine;
using UnityEngine.SceneManagement;
using RPG.Saving;

namespace RPG.SceneManagement
{
    public class Portal : SceneTransition, ISaveable
    {
        enum DestinationIdentifier
        {
            A, B, C, D
        }

        [SerializeField] Transform spawnPoint = null;
        [SerializeField] DestinationIdentifier destination = DestinationIdentifier.A;
        private int currentScene;

        private void Awake() => currentScene = SceneManager.GetActiveScene().buildIndex;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player") return;

            StartCoroutine(Transition(UpdatePlayer));
        }

        private (Vector3, Quaternion) UpdatePlayer(GameObject player)
        {
            Portal[] scenePortals = FindObjectsOfType<Portal>();
            Portal destinationPortal = GetDestinationPortal(scenePortals);
            UpdateReturnScene(scenePortals, destinationPortal);

            return (destinationPortal.spawnPoint.position, destinationPortal.spawnPoint.rotation);
        }

        private Portal GetDestinationPortal(Portal[] scenePortals)
        {
            foreach (Portal portal in scenePortals)
            {
                if (portal == this || portal.destination != destination) continue;
                return portal;
            }

            Debug.LogError("No Portal found");
            return null;
        }

        private void UpdateReturnScene(Portal[] scenePortals, Portal destinationPortal)
        {
            foreach (Portal portal in scenePortals)
            {
                if (portal == this || portal == destinationPortal || portal.nextScene != destinationPortal.nextScene) continue;
                portal.nextScene = (RPGScenes)currentScene;
            }

            destinationPortal.nextScene = (RPGScenes)currentScene;
        }

        public object CaptureState()
        {
            return nextScene;
        }

        public void RestoreState(object state)
        {
            nextScene = (RPGScenes)state;
        }
    }
}
