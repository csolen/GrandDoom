using UnityEngine;
using TMPro;

public class Developer_UI_Text : MonoBehaviour
{
    public string currencyName;

    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (currencyName == "health")
        {
            text.text = "Health: " + PlayerController.instance.health.ToString();
        }
        else if (currencyName == "gold")
        {
            text.text = "Gold: " + PlayerController.instance.goldAmount.ToString();
        }
        else if (currencyName == "ammo")
        {
            text.text = "Ammo: " + PlayerController.instance.ammoAmount.ToString();
        }
        else if (currencyName == "enemies")
        {
            text.text = "Enemies: " + PlayerPrefs.GetInt("KilledEnemies").ToString() + " / " + PlayerPrefs.GetInt("TotalEnemyCount").ToString();
        }
        else if (currencyName == "killedEnemies")
        {
            text.text = "Enemies Killed: " + PlayerPrefs.GetInt("KilledEnemies").ToString();
        }
        else
        {
            Debug.LogWarning("No such thing as " + currencyName);
        }
    }
}
