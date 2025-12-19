using UnityEngine;

public enum Market_Item_Type
{
    Market_Refill_Health,
    Market_Refill_Ammo,
    Market_Select_RandomSkill
}

[CreateAssetMenu(fileName = "MarketItem_", menuName = "Game/Market Item")]
public class InGameMarketData : ScriptableObject
{
    [Header("General")]
    public string id;
    public string displayName;

    [Header("Visual")]
    public Sprite icon;

    [Header("Values")]
    public Market_Item_Type type;
    public int value;

}
