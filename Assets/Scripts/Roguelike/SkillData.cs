using UnityEngine;

public enum SkillType
{
    Damage,
    AttackSpeed,
    MoveSpeed,
    MaxAmmo,
    Health,
    Custom
}

[CreateAssetMenu(menuName = "Skills", fileName = "NewSkill")]
public class SkillData : ScriptableObject
{
    [Header("Genel")]
    public string id; 
    public string displayName;
    [TextArea] public string description;

    [Header("Görsel")]
    public Sprite icon;

    [Header("Değerler")]
    public SkillType type;
    public float value; 
}
