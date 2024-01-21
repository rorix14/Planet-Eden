using UnityEngine;

namespace RPG.SceneManagement
{
    public enum MenuButtonType
    {
        NEWGAME,
        CONTINUE,
        RESUME,
        LOADLASTSAVE,
        RETURNTOMENU,
        QUIT
    }

    public class MenuButton : MonoBehaviour
    {
        [SerializeField] private MenuButtonType buttonType;
        [SerializeField] private CanvasGroup border = null;
        public MenuButtonType ButtonType => buttonType;

        private void OnEnable()
        {
            transform.localScale = new Vector3(1, 1, 1);
            border.alpha = 0.5f;
        }
    }
}