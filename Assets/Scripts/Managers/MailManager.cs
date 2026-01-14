using UnityEngine;

public class MailManager : MonoBehaviour
{
    private MainMenuManager m_mainMenuManager;

    public GameObject mailMenu;
    public GameObject noNewMailsText;
    public GameObject TestMail;
    public GameObject[] Doticons;

    private bool isClaimed;

    private void Start()
    {
        m_mainMenuManager = GetComponent<MainMenuManager>();
        CalculateDotIcons();
    }

    public void OpenMailMenu()
    {
        m_mainMenuManager.CloseSettingsMenus();
        mailMenu.SetActive(true);
    }

    public void CloseMailMenu()
    {
        mailMenu.SetActive(false);
    }

    public void ClaimButton()
    {
        TestMail.SetActive(false);
        noNewMailsText.SetActive(true);
        isClaimed = true;
        CalculateDotIcons();
    }

    private void CalculateDotIcons()
    {
        for (int i = 0; i < Doticons.Length; i++)
        {
            if (!isClaimed)
            {
                Doticons[i].SetActive(true);
            }
            else
            {
                Doticons[i].SetActive(false);
            }
        }
    }
}
