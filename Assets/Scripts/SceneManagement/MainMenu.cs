using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.SceneManagement
{
    public class MainMenu : Menus
    {
        [SerializeField] private UnityEvent StartGameSound, NegationSound;
        [SerializeField] private AudioSource backGroudMusic = null;
        [SerializeField] private AudioSource confirmaionSoundClip = null;
        private bool isStartingGame;

        protected override void Start()
        {
            base.Start();
            LeanTween.alphaCanvas(GetComponent<CanvasGroup>(), 1, 1);
        }

        protected override void MenuButtonClick(GameObject button)
        {
            if (isStartingGame) return;

            switch (button.GetComponent<MenuButton>().ButtonType)
            {
                case MenuButtonType.NEWGAME:
                    StartNewGame();
                    break;
                case MenuButtonType.CONTINUE:
                    Continue();
                    break;
                case MenuButtonType.QUIT:
                    ConfirmationSound?.Invoke();
                    ConfirmQuitGame();
                    break;
            }
        }

        private void StartNewGame()
        {
            if (!savingWrapper.SavingSystem.CheckForFile(savingWrapper.DefaultSaveFile))
            {
                Transition();
                StartGameSound?.Invoke();
            }
            else
            {
                ConfirmationSound?.Invoke();
                ConfirmNewGame();
            }
        }

        private void Continue()
        {
            if (!savingWrapper.SavingSystem.CheckForFile(savingWrapper.DefaultSaveFile)) NegationSound?.Invoke();
 
            else
            {
                StartGameSound?.Invoke();
                Transition();
            }
        }

        private void ConfirmNewGame()
        {
            confirmationPopUp.Confirmation = () => 
            {
                savingWrapper.Delete();
                Transition();
            };

            confirmationPopUp.PopUpText.text = confirmationPopUp.NewGameConfirmationText;
            confirmationPopUp.gameObject.SetActive(true);
        }

        private void Transition()
        {
            isStartingGame = true;
            StartCoroutine(FadeOutSound(backGroudMusic, confirmaionSoundClip.clip.length));
            savingWrapper.LoadSceneFromMenu(confirmaionSoundClip.clip.length);
        }

        private IEnumerator FadeOutSound(AudioSource audioSource, float FadeTime)
        {
            float startVolume = audioSource.volume;

            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

                yield return null;
            }

            audioSource.Stop();
            audioSource.volume = startVolume;
        }
    }
}