using UnityEngine;
using TMPro;
using System;

public class EnergyUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Update")]
    [SerializeField] private float refreshInterval = 1f;

    private float t;

    private void OnEnable()
    {
        RefreshUI(forceUpdateEnergyFromTime: true);
    }

    private void Update()
    {
        if (EnergyManager.Instance == null) return;

        t += Time.unscaledDeltaTime;
        if (t >= refreshInterval)
        {
            t = 0f;
            RefreshUI(forceUpdateEnergyFromTime: true);
        }
    }

    private void RefreshUI(bool forceUpdateEnergyFromTime)
    {
        var em = EnergyManager.Instance;
        if (em == null) return;

        if (forceUpdateEnergyFromTime)
            em.UpdateEnergyFromTime();

        int energy = em.Energy;
        int max = EnergyManager.MaxEnergy;

        if (energyText != null)
            energyText.text = $"{energy}/{max}";

        if (timerText == null) return;

        if (energy >= max)
        {
            timerText.text = "Full";
            timerText.gameObject.SetActive(false);
            return;
        }

        timerText.gameObject.SetActive(true);

        TimeSpan remain = em.GetTimeToFull();
        timerText.text = FormatTime(remain);
    }

    private static string FormatTime(TimeSpan ts)
    {
        if (ts.TotalSeconds < 0) ts = TimeSpan.Zero;

        if (ts.TotalHours >= 1)
            return $"{(int)ts.TotalHours:00}:{ts.Minutes:00}:{ts.Seconds:00}";
        else
            return $"{ts.Minutes:00}:{ts.Seconds:00}";
    }
}
