using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.AI;
using RPG.Control;

namespace RPG.SceneManagement
{
    public abstract class SceneTransition : MonoBehaviour
    {
        [SerializeField] private float fadeOutTime = 1f;
        [SerializeField] private float fadeInTime = 0.5f;
        [SerializeField] protected RPGScenes nextScene;

        protected IEnumerator Transition(Func<GameObject, (Vector3, Quaternion)> OnSceneLoad = null)
        {
            if (nextScene < 0)
            {
                Debug.LogError("No scene was set");
                yield break;
            }

            DontDestroyOnLoad(gameObject);
            Fader fader = FindObjectOfType<Fader>();
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            GameObject player = GameObject.FindWithTag("Player");

            player.GetComponent<PlayerController>().enabled = false;

            yield return fader.Fade(1, fadeOutTime);

            savingWrapper.Save();

            yield return SceneManager.LoadSceneAsync((int)nextScene);

            player = GameObject.FindWithTag("Player");
            if (player) player.GetComponent<PlayerController>().enabled = false;

            savingWrapper.Load();

            if (OnSceneLoad != null && player)
            {
                (Vector3, Quaternion) posAndRot = OnSceneLoad.Invoke(player);
                UpdatePlayerPlace(player, posAndRot.Item1, posAndRot.Item2);
            }

            savingWrapper.Save();

            //yield return new WaitForSeconds(fadeWait);
            fader.Fade(0, fadeInTime);

            if (player) player.GetComponent<PlayerController>().enabled = true;

            Destroy(gameObject);
        }

        private void UpdatePlayerPlace(GameObject player, Vector3 position, Quaternion rotation)
        {
            player.GetComponent<NavMeshAgent>().Warp(position);
            player.transform.rotation = rotation;
        }
    }
}