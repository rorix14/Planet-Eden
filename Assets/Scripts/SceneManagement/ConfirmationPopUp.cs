using System;
using UnityEngine;
using TMPro;

public class ConfirmationPopUp : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI popUpText = null;
    [SerializeField] private string newGameConfirmationText = "Are you sure you want to start a new game? This will overwrite your current progress.";
    [SerializeField] private string loadGameConfirmationText = "Are you sure you want to load your last save?";
    [SerializeField] private string backToMenuConfirmationText = "Are you sure you want to return to the main menu? You will lose any unsaved progress.";
    [SerializeField] private string quitGameConfirmationText = "Are you sure you want to quit the game and go back to desktop?";
    public Action Confirmation = null;

    public TextMeshProUGUI PopUpText => popUpText;
    public string NewGameConfirmationText => newGameConfirmationText;
    public string LoadGameConfirmationText => loadGameConfirmationText;
    public string BackToMenuConfirmationText => backToMenuConfirmationText;
    public string QuitGameConfirmationText => quitGameConfirmationText;

    public void Confirm() => Confirmation?.Invoke();

    public void Cancel() => gameObject.SetActive(false);

    private void OnDisable() => Confirmation = null;
}