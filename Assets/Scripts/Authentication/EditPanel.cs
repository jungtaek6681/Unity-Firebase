using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditPanel : MonoBehaviour
{
    [SerializeField] PanelController panelController;

    [SerializeField] TMP_InputField nameInputField;
    [SerializeField] TMP_InputField passInputField;
    [SerializeField] TMP_InputField confirmInputField;

    [SerializeField] Button nameApplyButton;
    [SerializeField] Button passApplyButton;
    [SerializeField] Button backButton;
    [SerializeField] Button deleteButton;

    private void Awake()
    {
        nameApplyButton.onClick.AddListener(NameApply);
        passApplyButton.onClick.AddListener(PassApply);
        backButton.onClick.AddListener(Back);
        deleteButton.onClick.AddListener(Delete);
    }

    private void NameApply()
    {
        SetInteractable(false);

        Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile();
        profile.DisplayName = nameInputField.text;

        FirebaseManager.Auth.CurrentUser.UpdateUserProfileAsync(profile).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                panelController.ShowInfo("UpdateUserProfileAsync was canceled.");
                SetInteractable(true);
                return;
            }
            if (task.IsFaulted)
            {
                panelController.ShowInfo($"UpdateUserProfileAsync encountered an error: {task.Exception.Message}");
                SetInteractable(true);
                return;
            }

            panelController.ShowInfo("User profile updated successfully.");
            SetInteractable(true);
        });
    }

    private void PassApply()
    {
        SetInteractable(false);

        if (passInputField.text != confirmInputField.text)
        {
            panelController.ShowInfo("password doesn't matched");
            SetInteractable(true);
            return;
        }

        string newPassword = passInputField.text;

        FirebaseManager.Auth.CurrentUser.UpdatePasswordAsync(newPassword).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                panelController.ShowInfo("UpdatePasswordAsync was canceled.");
                SetInteractable(true);
                return;
            }
            if (task.IsFaulted)
            {
                panelController.ShowInfo($"UpdatePasswordAsync encountered an error: {task.Exception.Message}");
                SetInteractable(true);
                return;
            }

            panelController.ShowInfo("Password updated successfully.");
            SetInteractable(true);
        });
    }

    private void Back()
    {
        panelController.SetActivePanel(PanelController.Panel.Main);
    }

    private void Delete()
    {
        FirebaseManager.Auth.CurrentUser.DeleteAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                panelController.ShowInfo("DeleteAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                panelController.ShowInfo($"DeleteAsync encountered an error: {task.Exception.Message}");
                return;
            }

            panelController.ShowInfo("User deleted successfully.");
            panelController.SetActivePanel(PanelController.Panel.Login);
        });
    }

    private void SetInteractable(bool interactable)
    {
        nameInputField.interactable = interactable;
        passInputField.interactable = interactable;
        confirmInputField.interactable = interactable;
        nameApplyButton.interactable = interactable;
        passApplyButton.interactable = interactable;
        backButton.interactable = interactable;
        deleteButton.interactable = interactable;
    }
}
