using System.Collections.Generic;
using UnityEngine;

public class RoguelikeManager : MonoBehaviour
{
    [Header("Xp")]
    public int xpThreshold = 100;

    [Header("UI")]
    public GameObject selectionPanel;
    public Transform cardsParent;
    public SkillOptionUI cardPrefab;

    [Header("Skill Pool")]
    public List<SkillData> allSkills;

    public GameObject delayerImg;

    bool isMenuOpen;
    readonly List<SkillOptionUI> spawnedCards = new();

    private void Awake()
    {
        ResetAllSkillLevels();
    }

    private void ResetAllSkillLevels()
    {
        foreach (var skill in allSkills)
        {
            if (!string.IsNullOrEmpty(skill.levelGroupId))
            {
                string key = "SkillLevel_" + skill.levelGroupId;
                PlayerPrefs.SetInt(key, 0);
            }
        }
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
            card.Setup(chosenSkill, OnSkillSelected);

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
            case SkillType.Health:
                break;

            case SkillType.MoveSpeed:
                break;

            case SkillType.Damage:
                break;

            case SkillType.AttackSpeed:
                break;

            case SkillType.MaxAmmo:
                break;

            case SkillType.Custom:
                break;
        }
    }

    public void RollCards()
    {
        ClearOldCards();
        SpawnRandomSkillCards(3);
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
