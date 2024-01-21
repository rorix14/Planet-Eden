using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
using RPG.Control;

namespace RPG.Cinamatics
{
    public class CinamaticControlRemover : MonoBehaviour
    {
        [SerializeField] private GameObject[] UIToRemove = null;
        [SerializeField] private CanvasGroup mattes = null;
        private GameObject player = null;
        private PlayableDirector playableDirector = null;

        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
            playableDirector = GetComponent<PlayableDirector>();
        }

        private void Update()
        {
            if (playableDirector.state != PlayState.Playing) return;

            if (Input.GetMouseButtonDown(2)) playableDirector.Stop();
        }

        private void OnEnable()
        {
            playableDirector.played += DisableControl;
            playableDirector.played += TurnOnMovieMattes;
            playableDirector.stopped += EnableControl;
            playableDirector.stopped += TurnOffMovieMattes;
        }

        private void OnDisable()
        {
            playableDirector.played -= DisableControl;
            playableDirector.played -= TurnOnMovieMattes;
            playableDirector.stopped -= EnableControl;
            playableDirector.stopped += TurnOffMovieMattes;
        }

        private void DisableControl(PlayableDirector playableDirector)
        {
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;

            foreach (GameObject uiElement in UIToRemove) uiElement.SetActive(false);
        }

        private void TurnOnMovieMattes(PlayableDirector playableDirector)
        {
            mattes.gameObject.SetActive(true);
            LeanTween.alphaCanvas(mattes, 1, 0.5f);
        }

        private void TurnOffMovieMattes(PlayableDirector playableDirector)
        {
            LeanTween.alphaCanvas(mattes, 0, 0.3f).setOnComplete(() => mattes.gameObject.SetActive(false));
        }

        private void EnableControl(PlayableDirector playableDirector)
        {
            if (player == null) return;
            player.GetComponent<PlayerController>().enabled = true;

            foreach (GameObject uiElement in UIToRemove) uiElement.SetActive(true);
        }
    }
}