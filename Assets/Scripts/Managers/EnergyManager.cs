using UnityEngine;
using System;

public class EnergyManager : MonoBehaviour
{
    public const int MaxEnergy = 30;
    public const int EnergyChunk = 5;
    public static readonly TimeSpan RegenInterval = TimeSpan.FromMinutes(15);

    public int Energy { get; private set; } = MaxEnergy;

    private long nextChunkAtTicks = 0;

    private const string PP_ENERGY = "pp_energy";
    private const string PP_NEXT_TICKS = "pp_next_energy_ticks";

    public static EnergyManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadFromPrefs();
        UpdateEnergyFromTime();
        SaveToPrefs();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) SaveToPrefs();
    }

    private void OnApplicationQuit()
    {
        SaveToPrefs();
    }

    public bool SpendEnergy(int amount)
    {
        if (amount <= 0) return true;
        if (Energy < amount) return false;

        bool wasFull = (Energy >= MaxEnergy);

        Energy -= amount;

        if (wasFull && Energy < MaxEnergy)
        {
            nextChunkAtTicks = DateTime.UtcNow.Add(RegenInterval).Ticks;
        }

        SaveToPrefs();
        return true;
    }

    public void AddEnergy(int amount)
    {
        if (amount <= 0) return;

        Energy = Mathf.Min(MaxEnergy, Energy + amount);

        if (Energy >= MaxEnergy)
            nextChunkAtTicks = 0;

        SaveToPrefs();
    }

    public void UpdateEnergyFromTime()
    {
        if (Energy >= MaxEnergy)
        {
            nextChunkAtTicks = 0;
            return;
        }

        if (nextChunkAtTicks <= 0)
            nextChunkAtTicks = DateTime.UtcNow.Add(RegenInterval).Ticks;

        DateTime now = DateTime.UtcNow;
        DateTime next = new DateTime(nextChunkAtTicks, DateTimeKind.Utc);

        if (now < next) return;

        double intervalSec = RegenInterval.TotalSeconds;
        double passedSec = (now - next).TotalSeconds;

        int chunks = 1 + (int)(passedSec / intervalSec);
        int amount = chunks * EnergyChunk;

        Energy = Mathf.Min(MaxEnergy, Energy + amount);

        if (Energy >= MaxEnergy)
        {
            nextChunkAtTicks = 0;
        }
        else
        {
            nextChunkAtTicks = next.AddSeconds(chunks * intervalSec).Ticks;
        }
    }

    public TimeSpan GetTimeToNextEnergyChunk()
    {
        if (Energy >= MaxEnergy) return TimeSpan.Zero;

        if (nextChunkAtTicks <= 0)
            nextChunkAtTicks = DateTime.UtcNow.Add(RegenInterval).Ticks;

        DateTime now = DateTime.UtcNow;
        DateTime next = new DateTime(nextChunkAtTicks, DateTimeKind.Utc);

        TimeSpan remain = next - now;
        if (remain.TotalSeconds < 0) remain = TimeSpan.Zero;
        return remain;
    }

    public TimeSpan GetTimeToFull()
    {
        if (Energy >= MaxEnergy) return TimeSpan.Zero;

        int missing = MaxEnergy - Energy;
        int chunksMissing = Mathf.CeilToInt(missing / (float)EnergyChunk);

        TimeSpan toNext = GetTimeToNextEnergyChunk();
        if (chunksMissing <= 1) return toNext;

        return toNext + TimeSpan.FromSeconds((chunksMissing - 1) * RegenInterval.TotalSeconds);
    }

    private void LoadFromPrefs()
    {
        Energy = PlayerPrefs.GetInt(PP_ENERGY, MaxEnergy);

        string ticksStr = PlayerPrefs.GetString(PP_NEXT_TICKS, "0");
        long.TryParse(ticksStr, out nextChunkAtTicks);

        if (Energy >= MaxEnergy) nextChunkAtTicks = 0;
    }

    private void SaveToPrefs()
    {
        PlayerPrefs.SetInt(PP_ENERGY, Energy);
        PlayerPrefs.SetString(PP_NEXT_TICKS, nextChunkAtTicks.ToString());
        PlayerPrefs.Save();
    }
}
