using UnityEngine;

public class RoguelikeManager : MonoBehaviour
{
    public GameObject selectionPanel;

    private int xpCalculator;

    private void Update()
    {
        xpCalculator = PlayerPrefs.GetInt("Roguelike_Xp");

        if (xpCalculator >= 100)
        {
            PlayerPrefs.SetInt("Roguelike_Xp", 0);
            OpenSelectionMenu();
        }
    }

    public void OpenSelectionMenu()
    {
        Time.timeScale = 0f;
        selectionPanel.SetActive(true);
    }

    public void CloseSelectionMenu()
    {
        selectionPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
