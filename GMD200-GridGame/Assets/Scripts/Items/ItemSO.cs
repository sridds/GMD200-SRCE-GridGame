using NaughtyAttributes;
using UnityEngine;
using System.Reflection;
using System;

public abstract class ItemSO : ScriptableObject
{
    [Header("Item Settings")]
    [Tooltip("This is the name that will be shown in the inventory, shops, etc")]
    public string ItemName;

    [Tooltip("This is the description that will be shown in the inventory, shops, etc")]
    [TextArea]
    public string ItemDescription;

    [ShowAssetPreview]
    public Sprite ItemSprite;

    [Tooltip("The sell price if this item is listed in a shop")]
    public int SellPrice; // The buy price is not included in the scriptable object and is rather up to the shop listing class to be determined

    [Tooltip("The data that determines whether or not the item can be stacked")]
    public StackableData Stack;


    // this method must be overwritten by inheriting types to call the CloneGeneric method, passing in the type of item it is
    public abstract ItemSO Clone();

    #region Helpers
    /// <summary>
    /// Handles the cloning of a specified item inheritor
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    protected ItemSO CloneGeneric<T>() where T : ItemSO
    {
        T Instance = ScriptableObject.CreateInstance<T>();
        Instance.name = ItemName;

        return CopyValuesReflection(Instance);
    }

    /// <summary>
    /// Copies values inside the scriptable object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Instance"></param>
    /// <returns></returns>
    protected T CopyValuesReflection<T>(T Instance) where T : ItemSO
    {
        Type type = typeof(T);
        foreach(FieldInfo field in type.GetFields())
        {
            field.SetValue(Instance, field.GetValue(this));
        }

        return Instance;
    }
    #endregion
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

[System.Serializable]
public struct StackableData
{
    public bool CanStack;

    [AllowNesting, ShowIf(nameof(CanStack))]
    [Min(1), Tooltip("Indicates how many of this item can be stacked in the inventory")]
    public int MaxStack;
}
