using UnityEngine;
using UnityEngine.Events;

namespace RPG.UI
{
    public class SplashScreen : MonoBehaviour
    {
        [SerializeField] private CanvasGroup splashScreenText = null;
        [SerializeField] private CanvasGroup splashScreenContainer = null;
        [SerializeField] private CanvasGroup mainMenuContainer = null;
        [SerializeField] UnityEvent BackgroundMusic;
        [SerializeField] UnityEvent PressKeySound;
        private bool splashScreenEnabled = false;

        private void Awake()
        {
            splashScreenContainer.alpha = 0;
            splashScreenText.alpha = 0;
            mainMenuContainer.alpha = 0;
        }

        void Start()
        {
            LeanTween.alphaCanvas(splashScreenContainer, 1, 2).setOnComplete(ShowSplashScreenText);
            BackgroundMusic?.Invoke();
        }

        private void Update()
        {
            if (Input.anyKeyDown && splashScreenEnabled) SplashScreenTransition();
        }

        private void ShowSplashScreenText()
        {
            splashScreenEnabled = true;
            LeanTween.alphaCanvas(splashScreenText, 1, 2).setLoopPingPong();
        }

        private void SplashScreenTransition()
        {
            PressKeySound?.Invoke();
            splashScreenEnabled = false;
            LeanTween.alphaCanvas(splashScreenContainer, 0, 1).setOnComplete(ShowMainMenu);
        }

        private void ShowMainMenu()
        {
            mainMenuContainer.gameObject.SetActive(true);
            //gameObject.SetActive(false);
        }
    }
}
