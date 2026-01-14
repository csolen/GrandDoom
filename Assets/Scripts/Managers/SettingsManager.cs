using System;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    private MainMenuManager m_mainMenuManager;
    public GameObject settingsPanel;

    private readonly string privacyPolicy = "https://grand-attic.com/privacy";
    private readonly string termsOfService = "https://grand-attic.com/terms";

    private readonly string to = "support@grand-attic.gs";
    private readonly string subject = "Support Request";
    private readonly string body = "";

    private void Start()
    {
        m_mainMenuManager = GetComponent<MainMenuManager>();
    }

    public void OpenSettingsPanel()
    {
        m_mainMenuManager.CloseSettingsMenus();
        settingsPanel.SetActive(true);
    }

    public void CloseSettingsPanel()
    {
        settingsPanel.SetActive(false);
    }

    public void OpenURL(string urlName)
    {
        if (urlName == "privacy")
        {
            Application.OpenURL(privacyPolicy);
        }
        else if (urlName == "terms")
        {
            Application.OpenURL(termsOfService);
        }
    }

    public void ChangeSettingsButton(string keyName)
    {
        if (PlayerPrefs.GetInt(keyName) == 0)
        {
            PlayerPrefs.SetInt(keyName, 1);
        }
        else
        {
            PlayerPrefs.SetInt(keyName, 0);
        }
    }

    public void OpenSupportMail()
    {
        string mailUrl = $"mailto:{to}" + $"?subject={Uri.EscapeDataString(subject)}" + $"&body={Uri.EscapeDataString(body)}";
        Application.OpenURL(mailUrl);
    }
}
