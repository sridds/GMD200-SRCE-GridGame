using UnityEngine;

public abstract class ItemSO : ScriptableObject
{
    public string ItemName;

    // The buy price is not included in the scriptable object and is rather up to the shop listing class to be determined
    public int SellPrice;
}
