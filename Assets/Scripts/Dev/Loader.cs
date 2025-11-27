using UnityEngine;

public class Loader : MonoBehaviour
{
    private void Awake()
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;

        PlayerPrefs.SetInt("TotalEnemyCount", 0);
        PlayerPrefs.SetInt("KilledEnemies", 0);
    }
}
