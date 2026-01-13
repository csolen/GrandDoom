using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;

    [Header("Defaults (first run)")]
    public int diamondCount = 100;
    public int silverCount = 1000;
    public int chapterCount = 1;

    const string K_Diamond = "currency_diamond";
    const string K_Silver = "currency_silver";
    const string K_Chapter = "progress_chapter";

    void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }

        instance = this;
        DontDestroyOnLoad(gameObject);

        Load();
    }

    public void AddDiamond(int amount)
    {
        diamondCount += amount;
        Save();
    }

    public void AddSilver(int amount)
    {
        silverCount += amount;
        Save();
    }

    public void SetChapter(int chapter)
    {
        chapterCount += chapter;
        Save();
    }

    public void Save()
    {
        PlayerPrefs.SetInt(K_Diamond, diamondCount);
        PlayerPrefs.SetInt(K_Silver, silverCount);
        PlayerPrefs.SetInt(K_Chapter, chapterCount);
        PlayerPrefs.Save(); Load();
    }

    public void Load()
    {
        diamondCount = PlayerPrefs.GetInt(K_Diamond, diamondCount);
        silverCount = PlayerPrefs.GetInt(K_Silver, silverCount);
        chapterCount = PlayerPrefs.GetInt(K_Chapter, chapterCount);
    }

    void OnApplicationPause(bool paused)
    {
        if (paused) Save();
    }

    void OnApplicationQuit()
    {
        Save();
    }

    public void ResetSave()
    {
        PlayerPrefs.DeleteKey(K_Diamond);
        PlayerPrefs.DeleteKey(K_Silver);
        PlayerPrefs.DeleteKey(K_Chapter);
        PlayerPrefs.Save();
        Load();
    }
}
