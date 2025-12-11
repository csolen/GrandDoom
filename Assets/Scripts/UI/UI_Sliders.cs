using UnityEngine;
using UnityEngine.UI;

public class UI_Sliders : MonoBehaviour
{
    public enum BarType
    {
        Health,
        XP
    }

    public BarType barType = BarType.Health;
    public float lerpSpeed = 5f;

    public Slider slider;
    public Image frontFill;
    public Image backFill;

    int xpRequiredForLevel;
    float targetValue;
    float frontValue;
    float backValue;
    float previousRawValue;
    bool isHealing;
    bool initialized;

    void Start()
    {
        xpRequiredForLevel = Mathf.Max(1, PlayerPrefs.GetInt("Roguelike_Required_Xp", 100));
    }

    void Update()
    {
        if (barType == BarType.Health)
        {
            if (PlayerController.instance == null)
                return;

            float current = PlayerController.instance.health;
            float max = PlayerController.instance.maxHealth;
            float raw = Mathf.Clamp01(current / max);

            if (!initialized)
            {
                targetValue = raw;
                frontValue = raw;
                backValue = raw;
                previousRawValue = raw;
                slider.value = raw;
                frontFill.fillAmount = raw;
                backFill.fillAmount = raw;
                initialized = true;
                return;
            }

            if (!Mathf.Approximately(raw, previousRawValue))
            {
                isHealing = raw > previousRawValue;
                previousRawValue = raw;
                targetValue = raw;

                if (isHealing)
                {
                    backValue = targetValue;
                }
                else
                {
                    frontValue = targetValue;
                }
            }

            if (isHealing)
            {
                frontValue = Mathf.Lerp(frontValue, targetValue, lerpSpeed * Time.deltaTime);
            }
            else
            {
                backValue = Mathf.Lerp(backValue, targetValue, lerpSpeed * Time.deltaTime);
            }

            frontFill.fillAmount = frontValue;
            backFill.fillAmount = backValue;
            slider.value = frontValue;
        }
        else if (barType == BarType.XP)
        {
            int xp = PlayerPrefs.GetInt("Roguelike_Xp", 0);
            xpRequiredForLevel = Mathf.Max(1, PlayerPrefs.GetInt("Roguelike_Required_Xp", xpRequiredForLevel));

            float raw = Mathf.Clamp01((float)xp / xpRequiredForLevel);

            if (!initialized)
            {
                targetValue = raw;
                frontValue = raw;
                backValue = raw;
                slider.value = raw;
                frontFill.fillAmount = raw;
                backFill.fillAmount = raw;
                initialized = true;
            }

            if (xp == 0)
            {
                targetValue = 0f;
                frontValue = 0f;
                backValue = 0f;
                frontFill.fillAmount = 0f;
                backFill.fillAmount = 0f;
                slider.value = 0f;
                return;
            }

            targetValue = raw;
            frontValue = targetValue;
            backValue = Mathf.Lerp(backValue, targetValue, lerpSpeed * Time.deltaTime);

            frontFill.fillAmount = frontValue;
            backFill.fillAmount = backValue;
            slider.value = frontValue;
        }
    }
}
