using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoguelikeManager : MonoBehaviour
{
    [Header("Xp")]
    public int xpThreshold = 100;

    [Header("UI")]
    public GameObject selectionPanel;
    public Transform cardsParent;
    public SkillOptionUI cardPrefab;
    public GameObject delayerImg;

    bool isMenuOpen;
    readonly List<SkillOptionUI> spawnedCards = new();

    [Header("Re-Roll Button")]
    public GameObject reRollButton;
    public TextMeshProUGUI reRollButtonText;
    private int reRollCount = 0;

    [Header("Skill Pool")]
    public List<SkillData> allSkills;

    private void Awake()
    {
        ResetAllSkillLevels();
    }

    void Update()
    {
        if (isMenuOpen) return;

        int xpCalculator = PlayerPrefs.GetInt("Roguelike_Xp", 0);

        if (xpCalculator >= xpThreshold)
        {
            PlayerPrefs.SetInt("Roguelike_Xp", 0);

            delayerImg.SetActive(true);
            OpenSelectionMenu();
        }
    }

    private void ResetAllSkillLevels()
    {
        if (PlayerPrefs.GetInt("RerollButtonFreeState") == 0)
        {
            reRollButtonText.text = "Re-roll (1)";
        }

        foreach (var skill in allSkills)
        {
            if (!string.IsNullOrEmpty(skill.levelGroupId))
            {
                string key = "SkillLevel_" + skill.levelGroupId;
                PlayerPrefs.SetInt(key, 0);
            }
        }
    }

    void ClickDelayer()
    {
        delayerImg.SetActive(false);
    }

    public void OpenSelectionMenu()
    {
        isMenuOpen = true;

        Invoke(nameof(ClickDelayer), 0.45f);

        PlayerPrefs.SetInt("ShouldStopTheGame", 1);

        ShowCursorInEditor(true);

        selectionPanel.SetActive(true);

        if (reRollCount >= 2)
        {
            reRollButton.SetActive(false);
        }
        else
        {
            reRollButton.SetActive(true);
        }

        RollCards();
    }

    public void CloseSelectionMenu()
    {
        selectionPanel.SetActive(false);

        PlayerPrefs.SetInt("ShouldStopTheGame", 0);

        isMenuOpen = false;

        ShowCursorInEditor(false);
        ClearOldCards();
    }

    void ClearOldCards()
    {
        foreach (var card in spawnedCards)
        {
            if (card != null)
                Destroy(card.gameObject);
        }
        spawnedCards.Clear();
    }

    List<SkillData> GetAvailableSkills()
    {
        List<SkillData> result = new();

        foreach (var skill in allSkills)
        {
            if (CanSpawnSkill(skill))
                result.Add(skill);
        }

        return result;
    }

    bool CanSpawnSkill(SkillData skill)
    {
        if (skill == null) return false;

        if (string.IsNullOrEmpty(skill.levelGroupId))
            return true;

        int currentLevel = GetCurrentLevel(skill.levelGroupId);

        if (currentLevel >= skill.maxLevel)
            return false;

        if (currentLevel <= 0)
            return skill.levelIndex == 1;

        return skill.levelIndex == currentLevel + 1;
    }

    int GetCurrentLevel(string groupId)
    {
        string key = "SkillLevel_" + groupId;
        return PlayerPrefs.GetInt(key, 0);
    }

    void SetCurrentLevel(string groupId, int newLevel)
    {
        string key = "SkillLevel_" + groupId;
        PlayerPrefs.SetInt(key, newLevel);
    }

    void SpawnRandomSkillCards(int count)
    {
        List<SkillData> pool = GetAvailableSkills();
        if (pool.Count == 0)
        {
            CloseSelectionMenu();
            return;
        }

        int finalCount = Mathf.Min(count, pool.Count);

        for (int i = 0; i < finalCount; i++)
        {
            int index = Random.Range(0, pool.Count);
            SkillData chosenSkill = pool[index];
            pool.RemoveAt(index);

            SkillOptionUI card = Instantiate(cardPrefab, cardsParent);

            int currentLevel = string.IsNullOrEmpty(chosenSkill.levelGroupId)
                ? 0
                : GetCurrentLevel(chosenSkill.levelGroupId);

            card.Setup(chosenSkill, currentLevel, OnSkillSelected);

            spawnedCards.Add(card);
        }
    }

    void OnSkillSelected(SkillData chosenSkill)
    {
        if (!string.IsNullOrEmpty(chosenSkill.levelGroupId))
        {
            int currentLevel = GetCurrentLevel(chosenSkill.levelGroupId);
            if (chosenSkill.levelIndex > currentLevel)
                SetCurrentLevel(chosenSkill.levelGroupId, chosenSkill.levelIndex);
        }

        ApplySkillToPlayer(chosenSkill);
        CloseSelectionMenu();
    }

    void ApplySkillToPlayer(SkillData skill)
    {
        switch (skill.type)
        {
            case SkillType.MaxHealth:
                Debug.Log("Max Health");
                break;
            case SkillType.MaxAmmo:
                Debug.Log("Max Ammo");
                break;
            case SkillType.CurrentHealth:
                Debug.Log("Current Health");
                break;
            case SkillType.CurrentAmmo:
                Debug.Log("Current Ammo");
                break;
            case SkillType.PlayerDamage:
                Debug.Log("Player Damage");
                break;
            case SkillType.PlayerMoveSpeed:
                Debug.Log("Player Move Speed");
                break;
            case SkillType.RequiredXpAmount:
                Debug.Log("Required XP Amount");
                break;
            case SkillType.EnemyDropChance:
                Debug.Log("Enemy Drop Chance");
                break;
            case SkillType.EnemyDamage:
                Debug.Log("Enemy Damage");
                break;
            case SkillType.Lifesteal:
                Debug.Log("LifeSteal");
                break;
            case SkillType.EnemySpeed:
                Debug.Log("Enemy Speed");
                break;
        }
    }

    private void RollCards()
    {
        ClearOldCards();
        SpawnRandomSkillCards(3);
    }

    public void ReRollCardsButton()
    {
        reRollCount++;
        reRollButtonText.text = "Re-roll " + "20 Golds";

        if (reRollCount >= 2)
        {
            reRollButton.SetActive(false);
            reRollCount = 1;
        }

        RollCards();
    }

    void ShowCursorInEditor(bool state)
    {
#if UNITY_EDITOR
        if (state)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
#endif
    }
}
