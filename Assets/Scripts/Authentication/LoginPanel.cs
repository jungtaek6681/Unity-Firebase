using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{
    [SerializeField] PanelController panelController;

    [SerializeField] TMP_InputField emailInputField;
    [SerializeField] TMP_InputField passInputField;

    [SerializeField] Button signUpButton;
    [SerializeField] Button loginButton;
    [SerializeField] Button resetPasswordButton;

    private void Awake()
    {
        signUpButton.onClick.AddListener(SignUp);
        loginButton.onClick.AddListener(Login);
        resetPasswordButton.onClick.AddListener(ResetPassword);
    }

    public void SignUp()
    {
        panelController.SetActivePanel(PanelController.Panel.SignUp);
    }

    private void ResetPassword()
    {
        panelController.SetActivePanel(PanelController.Panel.Reset);
    }

    public void Login()
    {
        SetInteractable(false);

        string id = emailInputField.text;
        string pass = passInputField.text;

        FirebaseManager.Auth.SignInWithEmailAndPasswordAsync(id, pass).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                panelController.ShowInfo("SignInWithEmailAndPasswordAsync was canceled.");
                SetInteractable(true);
                return;
            }
            if (task.IsFaulted)
            {
                panelController.ShowInfo($"SignInWithEmailAndPasswordAsync encountered an error: {task.Exception.Message}");
                SetInteractable(true);
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.Log($"User signed in successfully: {result.User.DisplayName} ({result.User.UserId})");
            if (result.User.IsEmailVerified)
            {
                panelController.SetActivePanel(PanelController.Panel.Main);
            }
            else
            {
                panelController.SetActivePanel(PanelController.Panel.Verify);
            }
            SetInteractable(true);
        });
    }

    private void SetInteractable(bool interactable)
    {
        emailInputField.interactable = interactable;
        passInputField.interactable = interactable;
        signUpButton.interactable = interactable;
        loginButton.interactable = interactable;
        resetPasswordButton.interactable = interactable;
    }
}
