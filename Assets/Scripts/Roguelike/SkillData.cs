using UnityEngine;

public enum SkillType
{
    Damage,
    AttackSpeed,
    MoveSpeed,
    Health,
    MaxAmmo,
    Custom
}

public enum SkillRarity
{
    Common,
    Rare,
    Legendary
}

[CreateAssetMenu(menuName = "MyGame/Skill", fileName = "NewSkill")]
public class SkillData : ScriptableObject
{
    [Header("General")]
    public string id;
    public string displayName;
    [TextArea] public string description;

    [Header("Visual")]
    public Sprite icon;

    [Header("Values")]
    public SkillType type;
    public float value;

    [Header("Rarity")]
    public SkillRarity rarity = SkillRarity.Common;

    [Header("Level")]
    public string levelGroupId;
    public int levelIndex = 1;
    public int maxLevel = 1;
}
