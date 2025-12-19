using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MarketItemOptionUI : MonoBehaviour
{
    [Header("UI")]
    public Image background;
    public Image titleArea;
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public Button selectButton;
    public Image iconImage;

    public TextMeshProUGUI buttonPriceText;

    [Header("Rarity Colors (SkillData ile birebir)")]
    public RarityColor[] rarityColors;

    InGameMarketData data;
    Action<InGameMarketData> onSelected;

    public void Setup(InGameMarketData data, Action<InGameMarketData> onSelected)
    {
        this.data = data;
        this.onSelected = onSelected;

        if (titleText != null)
            titleText.text = data.displayName;

        if (descriptionText != null)
            descriptionText.text = data.description;

        if (iconImage != null)
            iconImage.sprite = data.icon;

        if (buttonPriceText != null)
            buttonPriceText.text = data.price.ToString();

        ApplyRarityColors(data.rarity);

        if (selectButton != null)
        {
            selectButton.onClick.RemoveAllListeners();
            selectButton.onClick.AddListener(() =>
                this.onSelected?.Invoke(this.data));
        }
    }

    void ApplyRarityColors(RarityType rarity)
    {
        for (int i = 0; i < rarityColors.Length; i++)
        {
            if (rarityColors[i].rarity == rarity)
            {
                if (background != null)
                    background.color = rarityColors[i].backgroundColor;

                if (titleArea != null)
                    titleArea.color = rarityColors[i].titleAreaColor;

                return;
            }
        }
    }
}
