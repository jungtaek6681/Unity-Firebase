using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResetPanel : MonoBehaviour
{
    [SerializeField] PanelController panelController;

    [SerializeField] TMP_InputField emailInputField;

    [SerializeField] Button sendButton;
    [SerializeField] Button cancelButton;

    private void Awake()
    {
        sendButton.onClick.AddListener(SendResetMail);
        cancelButton.onClick.AddListener(Cancel);
    }

    private void SendResetMail()
    {
        SetInteractable(false);

        string emailAddress = emailInputField.text;

        FirebaseManager.Auth.SendPasswordResetEmailAsync(emailAddress).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                panelController.ShowInfo("SendPasswordResetEmailAsync was canceled.");
                SetInteractable(true);
                return;
            }
            if (task.IsFaulted)
            {
                panelController.ShowInfo($"SendPasswordResetEmailAsync encountered an error: {task.Exception.Message}");
                SetInteractable(true);
                return;
            }

            panelController.ShowInfo("Password reset email sent successfully.");
            panelController.SetActivePanel(PanelController.Panel.Login);
            SetInteractable(true);
        });
    }

    private void Cancel()
    {
        panelController.SetActivePanel(PanelController.Panel.Login);
    }

    private void SetInteractable(bool interactable)
    {
        emailInputField.interactable = interactable;
        sendButton.interactable = interactable;
        cancelButton.interactable = interactable;
    }
}
