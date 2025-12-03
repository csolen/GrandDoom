using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SkillOptionUI : MonoBehaviour
{
    public Image iconImage;
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public TMP_Text rarityText;
    public Image backgroundImage;
    public Button selectButton;

    SkillData _skillData;
    Action<SkillData> _onSelected;

    public void Setup(SkillData data, Action<SkillData> onSelected)
    {
        _skillData = data;
        _onSelected = onSelected;

        if (iconImage != null) iconImage.sprite = data.icon;
        if (titleText != null) titleText.text = data.displayName;
        if (descriptionText != null) descriptionText.text = data.description;

        Color rarityColor = GetColorForRarity(data.rarity);

        if (rarityText != null)
        {
            rarityText.text = data.rarity.ToString();
            rarityText.color = rarityColor;
        }

        if (backgroundImage != null)
        {
            Color c = rarityColor;
            c.a = 0.35f;
            backgroundImage.color = c;
        }

        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(OnClick);
    }

    Color GetColorForRarity(SkillRarity rarity)
    {
        switch (rarity)
        {
            case SkillRarity.Common: return Color.blue;
            case SkillRarity.Rare: return Color.green;
            case SkillRarity.Legendary: return Color.yellow;
        }
        return Color.white;
    }

    void OnClick()
    {
        _onSelected?.Invoke(_skillData);
    }
}
