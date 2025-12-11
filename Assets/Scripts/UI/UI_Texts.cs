using UnityEngine;
using TMPro;

public class UI_Texts : MonoBehaviour
{
    public string whatUI;

    private TextMeshProUGUI UI_Text;

    private void Awake()
    {
        UI_Text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (whatUI == "health")
        {
            UI_Text.text = "Health: " + PlayerController.instance.health.ToString() + "/" + PlayerController.instance.maxHealth.ToString();
        }
        else if (whatUI == "gold")
        {
            UI_Text.text = "Gold: " + PlayerController.instance.goldAmount.ToString();
        }
        else if (whatUI == "ammo")
        {
            UI_Text.text = "Ammo: " + PlayerController.instance.ammoAmount.ToString() + " / " + PlayerController.instance.maxAmmoAmount.ToString();
        }
        else if (whatUI == "enemies")
        {
            UI_Text.text = "Enemies: " + PlayerPrefs.GetInt("KilledEnemies").ToString() + " / " + PlayerPrefs.GetInt("TotalEnemyCount").ToString();
        }
        else if (whatUI == "killedEnemies")
        {
            UI_Text.text = PlayerPrefs.GetInt("KilledEnemies").ToString();
        }
        else if (whatUI == "xp")
        {
            UI_Text.text = "Xp: " + PlayerPrefs.GetInt("Roguelike_Xp").ToString() + " / " + PlayerPrefs.GetInt("Roguelike_Required_Xp").ToString();
        }
        else if (whatUI == "levelTimer")
        {
            UI_Text.text = "Time: " + PlayerPrefs.GetString("LevelTimer");
        }
        else
        {
            Debug.LogWarning("No such thing as " + whatUI);
        }
    }
}
