using UnityEngine;

namespace RPG.Core
{
    public class PersistentObjectSpawer : MonoBehaviour
    {
        [SerializeField] GameObject persistentObjectPrefab = null;
        static bool hasSpawned;

        private void Awake()
        {
            if (hasSpawned) return;

            SpawnPersistentObjects();
            hasSpawned = true;
        }

        private void SpawnPersistentObjects() => DontDestroyOnLoad(Instantiate(persistentObjectPrefab));
    }
}

