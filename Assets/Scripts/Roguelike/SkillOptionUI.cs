using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SkillOptionUI : MonoBehaviour
{
    [Header("UI Referansları")]
    public Image iconImage;
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public Button selectButton;

    private SkillData _skillData;
    private Action<SkillData> _onSelected;

    // Bu, kartın dışarıdan kurulmasını sağlıyor
    public void Setup(SkillData data, Action<SkillData> onSelected)
    {
        _skillData = data;
        _onSelected = onSelected;

        if (iconImage != null) iconImage.sprite = data.icon;
        if (titleText != null) titleText.text = data.displayName;
        if (descriptionText != null) descriptionText.text = data.description;

        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        _onSelected?.Invoke(_skillData);
    }
}
