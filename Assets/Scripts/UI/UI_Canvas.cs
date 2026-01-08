using UnityEngine;

public class UI_Canvas : MonoBehaviour
{
    public static UI_Canvas instance;

    public LuckyWheelController luckyWheelsPanel;
    public RoguelikeManager skillSelectionPanel;
    public In_Game_Market_Manager inGameMarket;
    public PauseGameManager pauseTheGamePanel;
    public GameObject deathScreen;
    public GameObject winScreen;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void OpenLuckyWheels()
    {
        luckyWheelsPanel.OpenWheelMenu();
    }

    public void OpenSkillSelection()
    {
        skillSelectionPanel.OpenSelectionMenu();
    }

    public void OpenInGameMarket()
    {
        inGameMarket.OpenInGameMarket();
    }

    public void OpenPauseMenu()
    {
        pauseTheGamePanel.OpenPauseGamePanel();
    }

    public void OpenDeathScreen()
    {
        deathScreen.SetActive(true);
    }

    public void OpenWinScreen()
    {
        winScreen.SetActive(true);
    }

    public void CloseLuckyWheels()
    {
        luckyWheelsPanel.CloseWheelMenu();
    }

    public void CloseSkillSelection()
    {
        skillSelectionPanel.CloseSelectionMenu();
    }

    public void CloseInGameMarket()
    {
        inGameMarket.CloseInGameMarket();
    }

    public void ClosePauseMenu()
    {
        pauseTheGamePanel.ClosePauseGamePanel();
    }

    public void CloseDeathScreen()
    {
        deathScreen.SetActive(false);
    }

    public void ShouldStopTheGame(bool state)
    {
        if (state)
        {
            PlayerPrefs.SetInt("ShouldStopTheGame", 1);
            PlayerController.instance.FreezePlayer();

#if UNITY_EDITOR

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
#endif
        }
        else
        {
            PlayerPrefs.SetInt("ShouldStopTheGame", 0);
            PlayerController.instance.UnFreezePlayer();

#if UNITY_EDITOR

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
#endif
        }
    }
}
