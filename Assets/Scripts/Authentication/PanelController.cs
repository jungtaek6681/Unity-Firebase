using UnityEngine;

public class PanelController : MonoBehaviour
{
    public enum Panel { Login, SignUp, Verify, Reset, Main, Edit }

    [SerializeField] InfoPanel infoPanel;
    [SerializeField] LoginPanel loginPanel;
    [SerializeField] SignUpPanel signUpPanel;
    [SerializeField] ResetPanel resetPanel;
    [SerializeField] VerifyPanel verifyPanel;
    [SerializeField] MainPanel mainPanel;
    [SerializeField] EditPanel editPanel;

    private void Start()
    {
        SetActivePanel(Panel.Login);
    }

    public void SetActivePanel(Panel panel)
    {
        loginPanel.gameObject.SetActive(panel == Panel.Login);
        signUpPanel.gameObject.SetActive(panel == Panel.SignUp);
        resetPanel.gameObject.SetActive(panel == Panel.Reset);
        mainPanel.gameObject.SetActive(panel == Panel.Main);
        editPanel.gameObject.SetActive(panel == Panel.Edit);
        verifyPanel.gameObject.SetActive(panel == Panel.Verify);
    }

    public void ShowInfo(string message)
    {
        infoPanel.ShowInfo(message);
    }
}
