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

    private bool isMenuOpen = false;
    private readonly List<SkillOptionUI> spawnedCards = new List<SkillOptionUI>();

    private void Update()
    {
        if (isMenuOpen) return;

        int xpCalculator = PlayerPrefs.GetInt("Roguelike_Xp", 0);

        if (xpCalculator >= xpThreshold)
        {
            PlayerPrefs.SetInt("Roguelike_Xp", 0);
            OpenSelectionMenu();
        }
    }

    public void OpenSelectionMenu()
    {
        isMenuOpen = true;

        Time.timeScale = 0f;

        ShowCursorInEditor(true);

        selectionPanel.SetActive(true);

        ClearOldCards();

        SpawnRandomSkillCards(3);
    }

    public void CloseSelectionMenu()
    {
        selectionPanel.SetActive(false);
        Time.timeScale = 1f;
        isMenuOpen = false;

        ShowCursorInEditor(false);
        ClearOldCards();
    }

    private void ClearOldCards()
    {
        foreach (var card in spawnedCards)
        {
            if (card != null)
                Destroy(card.gameObject);
        }
        spawnedCards.Clear();
    }

    private void SpawnRandomSkillCards(int count)
    {
        List<SkillData> pool = new List<SkillData>(allSkills);

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

    private void OnSkillSelected(SkillData chosenSkill)
    {
        ApplySkillToPlayer(chosenSkill);
        CloseSelectionMenu();
    }

    private void ApplySkillToPlayer(SkillData skill)
    {
        switch (skill.type)
        {
            case SkillType.Health:
                //PlayerController.instance.addHealth((int)skill.value);
                break;

            case SkillType.MoveSpeed:
                // PlayerController.instance.AddMoveSpeed(skill.value);
                break;

            case SkillType.Damage:
                // PlayerController.instance.AddDamage(skill.value);
                break;

            case SkillType.AttackSpeed:
                // PlayerController.instance.AddAttackSpeed(skill.value);
                break;

            case SkillType.MaxAmmo:
                // PlayerController.instance.AddAttackSpeed(skill.value);
                break;

            case SkillType.Custom:
                break;
        }
    }


    private void ShowCursorInEditor(bool state)
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
