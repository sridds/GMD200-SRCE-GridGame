using UnityEngine;

public abstract class Item : ScriptableObject
{
    // The buy price is not included in the scriptable object and is rather up to the shop listing class to be determined
    public int SellPrice;
}
