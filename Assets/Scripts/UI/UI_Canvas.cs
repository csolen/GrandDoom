using UnityEngine;

public class UI_Canvas : MonoBehaviour
{
    public static UI_Canvas instance;

    [Header("Panels")]
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

        SetLevelParameters();
    }

    private void SetLevelParameters()
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;

        PlayerPrefs.SetInt("ShouldStopTheGame", 0);
        PlayerPrefs.SetInt("TotalEnemyCount", 0);
        PlayerPrefs.SetInt("KilledEnemies", 0);
        PlayerPrefs.SetInt("Rouglike_Xp", 0);
        PlayerPrefs.SetInt("Roguelike_Required_Xp", skillSelectionPanel.requiredXP);
        PlayerPrefs.SetInt("RerollButtonFreeState", 0);

        PlayerPrefs.SetString("LevelTimer", "00:00:00");
    }

    public void LuckyWheelsPanelState(bool isActive)
    {
        if (isActive)
        {
            luckyWheelsPanel.OpenWheelMenu();
        }
        else
        {
            luckyWheelsPanel.CloseWheelMenu();
        }
    }

    public void SkillSelectionPanelState(bool isActive)
    {
        if (isActive)
        {
            skillSelectionPanel.OpenSelectionMenu();
        }
        else
        {
            skillSelectionPanel.CloseSelectionMenu();
        }
    }

    public void InGameMarketPanelState(bool isActive)
    {
        if (isActive)
        {
            inGameMarket.OpenInGameMarket();
        }
        else
        {
            inGameMarket.CloseInGameMarket();
        }
    }

    public void PauseMenuState(bool isActive)
    {
        if (isActive)
        {
            pauseTheGamePanel.OpenPauseGamePanel();
        }
        else
        {
            pauseTheGamePanel.ClosePauseGamePanel();
        }
    }

    public void DeathScreenState(bool isActive)
    {
        if (isActive)
        {
            deathScreen.SetActive(true);
        }
        else
        {
            deathScreen.SetActive(false);
        }
    }

    public void WinScreenState(bool isActive)
    {
        if (isActive)
        {
            winScreen.SetActive(true);
        }
        else
        {
            winScreen.SetActive(false);
        }
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
