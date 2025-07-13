using UnityEngine;

public class CreditsPanelManager : MonoBehaviour
{
    public GameObject creditsPanel;
    public GameObject mainMenuPanel;

    public void ShowCredits()
    {
        creditsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    public void HideCredits()
    {
        creditsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}
