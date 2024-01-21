using UnityEngine;
using System;

namespace RPG.SceneManagement
{
    public class GameMenu : Menus
    {
        public event Action OnResumeGame = null;

        protected override void MenuButtonClick(GameObject button)
        {
            switch (button.GetComponent<MenuButton>().ButtonType)
            {
                case MenuButtonType.RESUME:
                    ButtonClickedSound?.Invoke();
                    ResumeGame();
                    break;
                case MenuButtonType.LOADLASTSAVE:
                    ConfirmationSound?.Invoke();
                    ConfirmLoadGame();
                    break;
                case MenuButtonType.RETURNTOMENU:
                    ConfirmationSound?.Invoke();
                    ConfirmBackToMenu();
                    break;
                case MenuButtonType.QUIT:
                    ConfirmationSound?.Invoke();
                    ConfirmQuitGame();
                    break;
            }
        }

        private void ResumeGame()
        {
            OnResumeGame?.Invoke();
        }

        private void ConfirmLoadGame()
        {
            confirmationPopUp.Confirmation = () => { savingWrapper.LoadSceneFromMenu(); };
            confirmationPopUp.PopUpText.text = confirmationPopUp.LoadGameConfirmationText;
            confirmationPopUp.gameObject.SetActive(true);
        }

        private void ConfirmBackToMenu()
        {
            confirmationPopUp.Confirmation = () => { savingWrapper.GoToMainMenu(); };
            confirmationPopUp.PopUpText.text = confirmationPopUp.BackToMenuConfirmationText;
            confirmationPopUp.gameObject.SetActive(true);
        }
    }
}