using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignUpPanel : MonoBehaviour
{
    [SerializeField] PanelController panelController;

    [SerializeField] TMP_InputField emailInputField;
    [SerializeField] TMP_InputField passInputField;
    [SerializeField] TMP_InputField confirmInputField;

    [SerializeField] Button cancelButton;
    [SerializeField] Button signUpButton;

    private void Awake()
    {
        cancelButton.onClick.AddListener(Cancel);
        signUpButton.onClick.AddListener(SignUp);
    }

    public void SignUp()
    {
        SetInteractable(false);

        string id = emailInputField.text;
        string pass = passInputField.text;
        string confirm = confirmInputField.text;

        if (pass != confirm)
        {
            panelController.ShowInfo("Password doesn't matched");
            SetInteractable(true);
            return;
        }

        FirebaseManager.Auth.CreateUserWithEmailAndPasswordAsync(id, pass).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                panelController.ShowInfo("CreateUserWithEmailAndPasswordAsync was canceled.");
                SetInteractable(true);
                return;
            }
            if (task.IsFaulted)
            {
                panelController.ShowInfo($"CreateUserWithEmailAndPasswordAsync encountered an error: {task.Exception.Message}");
                SetInteractable(true);
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.AuthResult result = task.Result;
            panelController.ShowInfo($"Firebase user created successfully: {result.User.DisplayName} ({result.User.UserId})");
            panelController.SetActivePanel(PanelController.Panel.Login);
            SetInteractable(true);
        });
    }

    public void Cancel()
    {
        panelController.SetActivePanel(PanelController.Panel.Login);
    }

    private void SetInteractable(bool interactable)
    {
        emailInputField.interactable = interactable;
        passInputField.interactable = interactable;
        confirmInputField.interactable = interactable;
        cancelButton.interactable = interactable;
        signUpButton.interactable = interactable;
    }
}
