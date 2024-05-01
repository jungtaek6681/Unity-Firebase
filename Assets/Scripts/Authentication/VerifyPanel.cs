using Firebase.Extensions;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VerifyPanel : MonoBehaviour
{
    [SerializeField] PanelController panelController;

    [SerializeField] Button logoutButton;
    [SerializeField] Button sendButton;
    [SerializeField] TMP_Text sendButtonText;

    [SerializeField] int sendMailCooltime;

    private void Awake()
    {
        logoutButton.onClick.AddListener(Logout);
        sendButton.onClick.AddListener(SendVerifyMail);
    }

    private void OnEnable()
    {
        if (FirebaseManager.IsValid == false)
            return;

        StartCoroutine(VerifyCheckRoutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Logout()
    {
        FirebaseManager.Auth.SignOut();
        panelController.SetActivePanel(PanelController.Panel.Login);
    }

    private void SendVerifyMail()
    {
        StartCoroutine(VerifyMailCooltime());

        FirebaseManager.Auth.CurrentUser.SendEmailVerificationAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                panelController.ShowInfo("SendEmailVerificationAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                panelController.ShowInfo($"SendEmailVerificationAsync encountered an error: {task.Exception.Message}");
                return;
            }

            panelController.ShowInfo("Email sent successfully.");
        });
    }

    IEnumerator VerifyCheckRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);

            FirebaseManager.Auth.CurrentUser.ReloadAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("ReloadAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError($"ReloadAsync encountered an error: {task.Exception.Message}");
                    return;
                }

                if (FirebaseManager.Auth.CurrentUser.IsEmailVerified)
                {
                    panelController.SetActivePanel(PanelController.Panel.Main);
                }
            });
        }
    }

    IEnumerator VerifyMailCooltime()
    {
        int countDown = sendMailCooltime;

        sendButton.interactable = false;
        while (countDown > 0)
        {
            yield return new WaitForSeconds(1f);
            countDown--;
            sendButtonText.text = countDown.ToString();
        }
        sendButton.interactable = true;
        sendButtonText.text = "Send verify mail";
    }
}
