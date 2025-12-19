using UnityEngine;

public enum Market_Item_Type
{
    Market_Refill_Health,
    Market_Refill_Ammo,
    Market_Select_RandomSkill
}

public enum RarityType
{
    Common,
    Rare,
    Epic,
    Legendary
}

[System.Serializable]
public struct RarityColor
{
    public RarityType rarity;
    public Color backgroundColor;
    public Color titleAreaColor;
}

[CreateAssetMenu(fileName = "MarketItem_", menuName = "Game/Market Item")]
public class InGameMarketData : ScriptableObject
{
    [Header("General")]
    public string id;
    public string displayName;

    [Header("Description")]
    [TextArea(2, 4)]
    public string description;

    [Header("Visual")]
    public Sprite icon;

    [Header("Economy")]
    public int price;

    [Header("Type")]
    public Market_Item_Type type;

    [Header("Rarity")]
    public RarityType rarity;
}
