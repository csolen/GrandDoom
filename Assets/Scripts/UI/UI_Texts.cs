using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class UI_Texts : MonoBehaviour
{
    public string whatUI;

    private TextMeshProUGUI UI_Text;

    private int lastValue;
    private Coroutine scaleRoutine;

    private void Awake()
    {
        UI_Text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (whatUI == "health")
        {
            UI_Text.text = "Health: " + PlayerController.instance.health.ToString();
        }
        else if (whatUI == "gold")
        {
            UI_Text.text = "Gold: " + PlayerController.instance.goldAmount.ToString();
        }
        else if (whatUI == "ammo")
        {
            UI_Text.text = PlayerController.instance.ammoAmount.ToString();
        }
        else if (whatUI == "enemies")
        {
            UI_Text.text = "Enemies: " + PlayerPrefs.GetInt("KilledEnemies").ToString() + " / " + PlayerPrefs.GetInt("TotalEnemyCount").ToString();
        }
        else if (whatUI == "killedEnemies")
        {
            int currentValue = PlayerPrefs.GetInt("KilledEnemies");

            UI_Text.text = currentValue.ToString();

            if (currentValue != lastValue)
            {
                PlayScaleAnimation();
                lastValue = currentValue;
            }
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

    private void PlayScaleAnimation()
    {
        if (scaleRoutine != null)
            StopCoroutine(scaleRoutine);

        scaleRoutine = StartCoroutine(ScalePulse());
    }

    private IEnumerator ScalePulse()
    {
        Vector3 normalScale = Vector3.one;
        Vector3 bigScale = Vector3.one * 1.6f;

        float duration = 0.1f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            UI_Text.transform.localScale =
                Vector3.Lerp(normalScale, bigScale, t / duration);
            yield return null;
        }

        t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            UI_Text.transform.localScale =
                Vector3.Lerp(bigScale, normalScale, t / duration);
            yield return null;
        }
    }
}
