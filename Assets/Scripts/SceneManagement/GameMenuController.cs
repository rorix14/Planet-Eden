using UnityEngine.Events;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using RPG.Attributes;
using RPG.Core;

namespace RPG.SceneManagement
{
    public class GameMenuController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup menuTitle, menuButtons, background = null;
        [SerializeField] private UnityEvent GameOverSound, ButtonClickedSound;
        [SerializeField] private LeanTweenType easeType;
        private Health playerHealth = null;
        private GameMenu menu = null;
        private bool isGameOverActive = false;
        private TextMeshProUGUI menuTitleText = null;
        private CameraMovement cameraMovement = null;

        void Awake()
        {
            playerHealth = GameObject.FindWithTag("Player").GetComponent<Health>();
            menu = GetComponent<GameMenu>();
            background.alpha = 0;
            menuTitle.alpha = 0;
            menuButtons.alpha = 0;
            menuTitleText = menuTitle.GetComponent<TextMeshProUGUI>();
            cameraMovement = FindObjectOfType<CameraMovement>();
        }

        void Start()
        {
            menuTitle.gameObject.SetActive(false);
            background.gameObject.SetActive(false);
            menuButtons.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !menu.ConfirmationPopUp.gameObject.activeSelf && !playerHealth.IsDead)
            {
                TogglePauseMenu();
                ButtonClickedSound?.Invoke();
            }

            if (playerHealth.IsDead && !isGameOverActive)
            {
                isGameOverActive = true;
                ActivateGameOverTransition();
            }
        }

        private void ActivateGameOverTransition()
        {
            menuTitle.gameObject.SetActive(true);
            background.gameObject.SetActive(true);
            background.GetComponent<Image>().material = null;

            menuTitleText.text = "GAME OVER";
            menuTitleText.color = Color.red;
            
            LeanTween.alphaCanvas(background, 1, 2f).setEase(easeType);
            LeanTween.alphaCanvas(menuTitle, 1, 3f).setEase(easeType).setOnComplete(ActivateGameOverButtons);
            GameOverSound?.Invoke();
        }


        private void ActivateGameOverButtons()
        {
            foreach (MenuButton button in menu.Buttons)
            {
                if (button.ButtonType != MenuButtonType.RESUME) continue;

                button.gameObject.SetActive(false);
            }

            menuButtons.gameObject.SetActive(true);
            LeanTween.alphaCanvas(menuButtons, 1, 1);
        }

        private void TogglePauseMenu()
        {
            menuButtons.gameObject.SetActive(!menuButtons.gameObject.activeSelf);
            background.gameObject.SetActive(!background.gameObject.activeSelf);
            menuTitle.gameObject.SetActive(!menuTitle.gameObject.activeSelf);
            cameraMovement.UIElementStatus(this ,background.gameObject.activeSelf);

            if (menuButtons.gameObject.activeSelf)
            {
                menuTitle.alpha = 1;
                menuButtons.alpha = 1;

                background.alpha = 0.9f;

                menuTitleText.text = "PAUSED";
                menuTitleText.color = Color.white;

                foreach (MenuButton button in menu.Buttons)
                {
                    if (button.ButtonType != MenuButtonType.RESUME) continue;

                    button.gameObject.SetActive(true);
                }

                Time.timeScale = 0f;
            }
            else
            {
                background.alpha = 0f;
                menuTitle.alpha = 0f;
                menuButtons.alpha = 0f;
                Time.timeScale = 1f;
            }
        }

        private void OnEnable() => menu.OnResumeGame += TogglePauseMenu;

        private void OnDisable()
        {
            Time.timeScale = 1f;
            menu.OnResumeGame -= TogglePauseMenu;
        }
    }
}