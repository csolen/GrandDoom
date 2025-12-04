using UnityEngine;

public class Loader : MonoBehaviour
{
    private void Awake()
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;

        PlayerPrefs.SetInt("ShouldStopTheGame", 0);
        PlayerPrefs.SetInt("TotalEnemyCount", 0);
        PlayerPrefs.SetInt("KilledEnemies", 0);
        PlayerPrefs.SetInt("Rouglike_Xp", 0);
        PlayerPrefs.SetInt("RerollButtonFreeState", 0);

    }
}
