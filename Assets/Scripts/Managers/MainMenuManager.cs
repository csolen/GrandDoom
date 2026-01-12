using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject[] panels;

    [Header("Menu Buttons (same order as panels)")]
    [SerializeField] private Button[] menuButtons;

    [Header("Button Colors")]
    [SerializeField] private Color selectedColor = Color.white;
    [SerializeField] private Color normalColor = Color.gray;

    private TMP_Text[] buttonTmp;

    private void Awake()
    {
        buttonTmp = new TMP_Text[menuButtons.Length];
        for (int i = 0; i < menuButtons.Length; i++)
        {
            if (menuButtons[i] == null) continue;
            buttonTmp[i] = menuButtons[i].GetComponentInChildren<TMP_Text>(true);
        }
    }

    private void Start()
    {
        if (panels != null && panels.Length > 0)
            OpenPanel(0);
    }

    public void OpenPanel(int index)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            if (panels[i] != null)
                panels[i].SetActive(i == index);
        }

        for (int i = 0; i < menuButtons.Length; i++)
        {
            var btn = menuButtons[i];
            if (btn == null) continue;

            var graphic = btn.targetGraphic;
            if (graphic != null)
                graphic.color = (i == index) ? selectedColor : normalColor;

            if (buttonTmp != null && i < buttonTmp.Length && buttonTmp[i] != null)
                buttonTmp[i].gameObject.SetActive(i == index);
        }
    }

    public void BuyEnergy()
    {
        EnergyManager.Instance.AddEnergy(50);
    }

    public void StartLevel()
    {
        if (EnergyManager.Instance.Energy >= 5)
        {
            SceneManager.LoadScene("Level01");
            EnergyManager.Instance.SpendEnergy(5);
        }
        else
        {
            OpenPanel(2);
        }
    }
}
