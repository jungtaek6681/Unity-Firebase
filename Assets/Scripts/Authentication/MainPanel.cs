using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    [SerializeField] PanelController panelController;

    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text emailText;
    [SerializeField] TMP_Text idText;

    [SerializeField] Button logoutButton;
    [SerializeField] Button editButton;

    private void Awake()
    {
        logoutButton.onClick.AddListener(Logout);
        editButton.onClick.AddListener(Edit);
    }

    private void OnEnable()
    {
        if (FirebaseManager.IsValid == false)
            return;

        nameText.text = FirebaseManager.Auth.CurrentUser.DisplayName;
        emailText.text = FirebaseManager.Auth.CurrentUser.Email;
        idText.text = FirebaseManager.Auth.CurrentUser.UserId;
    }

    private void Logout()
    {
        FirebaseManager.Auth.SignOut();
        panelController.SetActivePanel(PanelController.Panel.Login);
    }

    private void Edit()
    {
        panelController.SetActivePanel(PanelController.Panel.Edit);
    }
}
