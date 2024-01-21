using UnityEngine;

namespace RPG.Utils
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType<T>();

                    if (!_instance) _instance = new GameObject("Instance of " + typeof(T)).AddComponent<T>();
                }

                return _instance;
            }
        }

        // clean if exists
        private void Awake()
        {
            if (_instance) Destroy(gameObject);
        }
    }
}

