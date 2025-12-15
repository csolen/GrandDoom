using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[Serializable]
public struct RarityColors
{
    public SkillRarity rarity;
    public Color backgroundColor;
    public Color titleAreaColor;
}

public class SkillOptionUI : MonoBehaviour
{
    public Image iconImage;
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public TMP_Text rarityText;

    public Image backgroundImage;
    public Image titleAreaImage;

    public Button selectButton;

    public Image[] cardLevelIcons;
    public Sprite cardLevelSpriteFull;

    [Header("Rarity Colors")]
    public RarityColors[] rarityColors;
    public Color cardLevelSpriteFullColor;

    SkillData skillData;
    Action<SkillData> onSelected;

    public void Setup(SkillData data, int currentLevel, Action<SkillData> onSelected)
    {
        skillData = data;
        this.onSelected = onSelected;

        iconImage.sprite = data.icon;
        titleText.text = data.displayName;
        descriptionText.text = data.description;

        rarityText.text = data.rarity.ToString();

        ApplyRarityColors(data.rarity);

        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(OnClick);

        SetupStars(data, currentLevel);
    }

    void ApplyRarityColors(SkillRarity rarity)
    {
        foreach (var rc in rarityColors)
        {
            if (rc.rarity != rarity)
                continue;

            Color bg = rc.backgroundColor;
            backgroundImage.color = bg;

            if (titleAreaImage != null)
                titleAreaImage.color = rc.titleAreaColor;

            return;
        }

        // fallback
        backgroundImage.color = Color.white;
        if (titleAreaImage != null)
            titleAreaImage.color = Color.white;
    }

    void SetupStars(SkillData data, int currentLevel)
    {
        if (string.IsNullOrEmpty(data.levelGroupId) || currentLevel <= 0)
        {
            SetAllStars(false);
            return;
        }

        int maxStars = Mathf.Clamp(data.maxLevel, 1, cardLevelIcons.Length);
        int clampedLevel = Mathf.Clamp(currentLevel, 0, maxStars);

        for (int i = 0; i < cardLevelIcons.Length; i++)
        {
            if (i >= maxStars)
            {
                cardLevelIcons[i].gameObject.SetActive(false);
                continue;
            }

            cardLevelIcons[i].gameObject.SetActive(true);

            if (i < clampedLevel)
            {
                cardLevelIcons[i].sprite = cardLevelSpriteFull;
                cardLevelIcons[i].color = cardLevelSpriteFullColor;
            }
            else
            {
                cardLevelIcons[i].color = Color.white;
            }
        }
    }

    void SetAllStars(bool state)
    {
        for (int i = 0; i < cardLevelIcons.Length; i++)
            cardLevelIcons[i].gameObject.SetActive(state);
    }

    void OnClick()
    {
        onSelected?.Invoke(skillData);
    }
}

