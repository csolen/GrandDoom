using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject[] panels;

    [Header("Menu Buttons")]
    [SerializeField] private Button[] menuButtons;

    [Header("Button Texts")]
    [SerializeField] private TMP_Text[] buttonTexts;

    [Header("Button Icons")]
    [SerializeField] private GameObject[] selectedIcons;
    [SerializeField] private GameObject[] unselectedIcons;

    [Header("Button Background Colors")]
    [SerializeField] private Color selectedColor = Color.white;
    [SerializeField] private Color normalColor = Color.gray;

    public GameObject settingsMenus;

    private void Start()
    {
        OpenPanel(0);
    }

    public void OpenPanel(int index)
    {
        for (int i = 0; i < panels.Length; i++)
            panels[i].SetActive(i == index);

        for (int i = 0; i < menuButtons.Length; i++)
        {
            var btn = menuButtons[i];
            btn.targetGraphic.color = (i == index) ? selectedColor : normalColor;

            if (buttonTexts != null && i < buttonTexts.Length && buttonTexts[i] != null)
                buttonTexts[i].gameObject.SetActive(i == index);

            if (selectedIcons != null && i < selectedIcons.Length && selectedIcons[i] != null)
                selectedIcons[i].SetActive(i == index);

            if (unselectedIcons != null && i < unselectedIcons.Length && unselectedIcons[i] != null)
                unselectedIcons[i].SetActive(i != index);
        }

        CloseSettingsMenus();
    }

    public void BuyEnergy()
    {
        EnergyManager.Instance.AddEnergy(50);
    }

    public void StartLevel()
    {
        if (EnergyManager.Instance.Energy >= 5)
        {
            SceneManager.LoadScene(CurrencyManager.instance.chapterCount);
            EnergyManager.Instance.SpendEnergy(5);
        }
        else
        {
            OpenPanel(2);
        }
    }

    public void OpenSettingsMenus()
    {
        settingsMenus.SetActive(true);
    }

    public void CloseSettingsMenus()
    {
        settingsMenus.SetActive(false);
    }
}
