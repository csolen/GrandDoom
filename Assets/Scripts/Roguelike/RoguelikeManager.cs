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
            OpenSelectionMenu();
        }

        if (PlayerPrefs.GetInt("Open_Roguelike") == 1)
        {
            OpenSelectionMenu();
        }
    }

    private void ResetAllSkillLevels()
    {
        if (PlayerPrefs.GetInt("RerollButtonFreeState") == 0)
        {
            reRollButtonText.text = "Re-roll (1)";
        }

        xpThreshold = PlayerPrefs.GetInt("Roguelike_Required_Xp", xpThreshold);

        PlayerPrefs.SetInt("Roguelike_Xp", 0);

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
        PlayerPrefs.SetInt("Open_Roguelike", 1);
        PlayerPrefs.SetInt("Roguelike_Xp", 0);
        delayerImg.SetActive(true);

        isMenuOpen = true;

        Invoke(nameof(ClickDelayer), 0.45f);

        GameTester.Instance.ShouldStopTheGame(true);

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
        PlayerPrefs.SetInt("Open_Roguelike", 0);
        selectionPanel.SetActive(false);

        isMenuOpen = false;

        GameTester.Instance.ShouldStopTheGame(false);
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
        List<EnemyController> allEnemies = GetAllEnemies();

        switch (skill.type)
        {
            case SkillType.MaxHealth:
                PlayerController.instance.maxHealth = PlayerController.instance.IncreaseByPercent(PlayerController.instance.maxHealth, (int)skill.value);
                break;
            case SkillType.MaxAmmo:
                PlayerController.instance.maxAmmoAmount = PlayerController.instance.IncreaseByPercent(PlayerController.instance.maxAmmoAmount, (int)skill.value);
                break;
            case SkillType.CurrentHealth:
                PlayerController.instance.AddHealth((int)skill.value);
                break;
            case SkillType.CurrentAmmo:
                PlayerController.instance.AddAmmo((int)skill.value);
                break;
            case SkillType.PlayerGunDamage:
                PlayerController.instance.playerDamage = PlayerController.instance.IncreaseByPercent(PlayerController.instance.playerDamage, (int)skill.value);
                break;
            case SkillType.PlayerSwordDamage:
                PlayerController.instance.katanaDamage = PlayerController.instance.IncreaseByPercent(PlayerController.instance.katanaDamage, (int)skill.value);
                break;
            case SkillType.PlayerMoveSpeed:
                PlayerController.instance.moveSpeed = PlayerController.instance.IncreaseByPercent(PlayerController.instance.moveSpeed, skill.value);
                break;
            case SkillType.RequiredXpAmount:
                xpThreshold = IncreaseByPercent(PlayerPrefs.GetInt("Roguelike_Required_Xp"), -(int)skill.value);
                PlayerPrefs.SetInt("Roguelike_Required_Xp", xpThreshold);
                break;
            case SkillType.EnemyDropChance:
                foreach (var enemy in allEnemies)
                {
                    enemy.dropChance = enemy.IncreaseByPercent(enemy.dropChance, skill.value);
                }
                break;
            case SkillType.EnemyDamage:
                foreach (var enemy in allEnemies)
                {
                    enemy.enemyDamage = enemy.IncreaseByPercent(enemy.enemyDamage, -(int)skill.value);
                }
                break;
            case SkillType.LifeStealChance:
                PlayerController.instance.lifeStealChance = PlayerController.instance.IncreaseByPercent(PlayerController.instance.lifeStealChance, skill.value);
                break;
            case SkillType.LifeStealAmount:
                PlayerController.instance.lifeStealAmount = PlayerController.instance.IncreaseByPercent(PlayerController.instance.lifeStealAmount, (int)skill.value);
                break;
            case SkillType.EnemySpeed:
                foreach (var enemy in allEnemies)
                {
                    enemy.chaseSpeed = enemy.IncreaseByPercent(enemy.chaseSpeed, -skill.value);
                }
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

    public int IncreaseByPercent(int value, int percent)
    {
        float result = value * (1f + percent / 100f);
        return Mathf.RoundToInt(result);

    }

    public List<EnemyController> GetAllEnemies()
    {
        List<EnemyController> enemies = new List<EnemyController>();

        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy_Holder");

        foreach (GameObject obj in enemyObjects)
        {
            EnemyController controller = obj.GetComponent<EnemyController>();
            if (controller != null)
            {
               enemies.Add(controller);
            }
        }

        return enemies;
    }

}
