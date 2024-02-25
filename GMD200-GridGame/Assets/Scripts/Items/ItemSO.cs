using NaughtyAttributes;
using UnityEngine;

public abstract class ItemSO : ScriptableObject
{
    [Tooltip("This is the name that will be shown in the inventory, shops, etc")]
    public string ItemName;

    // The buy price is not included in the scriptable object and is rather up to the shop listing class to be determined
    [Tooltip("The sell price if this item is listed in a shop")]
    public int SellPrice;
}

[System.Serializable]
public struct DurabilityData
{
    // Durability settings
    public bool HasDurability;

    [AllowNesting]
    [ShowIf(nameof(HasDurability))]
    public int MaxDurability;
}
