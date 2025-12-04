using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillOptionUI : MonoBehaviour
{
    public Image iconImage;
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public TMP_Text rarityText;
    public Image backgroundImage;
    public Button selectButton;

    public Image[] starImages;
    public Sprite fullStarSprite;

    SkillData skillData;
    Action<SkillData> onSelected;

    public void Setup(SkillData data, int currentLevel, Action<SkillData> onSelected)
    {
        skillData = data;
        this.onSelected = onSelected;

        iconImage.sprite = data.icon;
        titleText.text = data.displayName;
        descriptionText.text = data.description;

        Color rarityColor = GetColorForRarity(data.rarity);
        rarityText.text = data.rarity.ToString();
        rarityText.color = rarityColor;

        Color c = rarityColor;
        c.a = 0.35f;
        backgroundImage.color = c;

        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(OnClick);

        SetupStars(data, currentLevel);
    }

    void SetupStars(SkillData data, int currentLevel)
    {
        if (string.IsNullOrEmpty(data.levelGroupId))
        {
            for (int i = 0; i < starImages.Length; i++)
                starImages[i].gameObject.SetActive(false);

            return;
        }

        int maxStars = Mathf.Clamp(data.maxLevel, 1, starImages.Length);

        if (currentLevel <= 0)
        {
            for (int i = 0; i < starImages.Length; i++)
                starImages[i].gameObject.SetActive(false);

            return;
        }

        int clampedLevel = Mathf.Clamp(currentLevel, 0, maxStars);

        for (int i = 0; i < starImages.Length; i++)
        {
            if (i >= maxStars)
            {
                starImages[i].gameObject.SetActive(false);
                continue;
            }

            starImages[i].gameObject.SetActive(true);

            if (i < clampedLevel)
                starImages[i].sprite = fullStarSprite;
        }
    }

    Color GetColorForRarity(SkillRarity rarity)
    {
        switch (rarity)
        {
            case SkillRarity.Common:
                return Color.blue;
            case SkillRarity.Rare:
                return Color.green;
            case SkillRarity.Legendary:
                return Color.yellow;
        }
        return Color.white;
    }

    void OnClick()
    {
        onSelected(skillData);
    }
}
