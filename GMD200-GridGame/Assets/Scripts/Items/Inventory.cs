using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class Inventory : MonoBehaviour
{
    // the list of items
    [SerializeField]
    private List<Slot> items = new List<Slot>();

    [SerializeField]
    private ItemSO[] testItems;

    // equiped items. equipped items are not recorded in the item list
    private ArmorSO equippedArmor;
    private WeaponSO equippedWeapon;

    // accessors
    public ArmorSO EquippedArmor { get { return equippedArmor; } }
    public WeaponSO EquippedWeapon { get { return equippedWeapon; } }
    public List<Slot> Items { get { return items; } }

    // delegates

    public delegate void ArmorChanged(ArmorSO newArmor);
    public delegate void WeaponChanged(WeaponSO newWeapon);
    public delegate void InventoryUpdate(List<Slot> inv);

    public ArmorChanged OnArmorChanged;
    public WeaponChanged OnWeaponChanged;
    public InventoryUpdate OnInventoryUpdate;

    /// <summary>
    /// Returns true if the item was successfully added, returns false if the item could not be added.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool AddItem(ItemSO item)
    {
        // cannot be a null call
        if (item == null) return false;
        int index = items.Count;

        // if the item can stack, check to see if theres a matching item
        if (item.Stack.CanStack) {
            bool found = false;

            // iterate through each item and check if the items match
            foreach (Slot i in items)
            {
                if (i.Item == null) continue;

                // if item names match
                if (i.Item.ItemName == item.ItemName) {
                    // cannot exceed the maximum stack
                    if (i.Stack >= i.Item.Stack.MaxStack) continue;

                    // increase the stack if one was found
                    i.AddToStack();
                    index = items.IndexOf(i);
                    Debug.Log($"Added {item.ItemName} to item stack at index {index}. Stack count: {i.Stack}");

                    found = true;
                    break;
                }
            }

            // try to add the item without stacking
            if (!found)
                if (!TryAddItem(item))
                    return false;
        }
        else {
            // try to add the item without stacking
            if (!TryAddItem(item)) return false;
        }

        // invoke the successful item add event and return true
        OnInventoryUpdate?.Invoke(items);
        Debug.Log($"Added new {item.ItemName} to inventory at index {index}");

        return true;
    }

    /// <summary>
    /// Removes an item from the inventory
    /// </summary>
    /// <param name="item"></param>
    public void RemoveItem(int index)
    {
        if (!IsIndexValid(index)) return;

        Slot slot = items[index];

        // if the item is stacked, decrease the count
        Debug.Log($"Decreased stack of {slot.Item.ItemName} at index {index}");
        slot.RemoveFromStack();

        OnInventoryUpdate?.Invoke(items);
    }

    /// <summary>
    /// Equips weapon at corresponding index
    /// </summary>
    /// <param name="index"></param>
    public void EquipWeapon(int index)
    {
        WeaponSO prev = equippedWeapon;
        if (!Equip<WeaponSO>(index, ref equippedWeapon)) return;

        // invoke weapon change event
        OnWeaponChanged?.Invoke(equippedWeapon);
        OnInventoryUpdate?.Invoke(items);
    }

    /// <summary>
    /// Equips armor at the corresponding index
    /// </summary>
    /// <param name="index"></param>
    public void EquipArmor(int index)
    {
        ArmorSO prev = equippedArmor;
        if (!Equip<ArmorSO>(index, ref equippedArmor)) return;

        // invoke armor change event
        OnArmorChanged?.Invoke(equippedArmor);
        OnInventoryUpdate?.Invoke(items);
    }

    /// <summary>
    /// You pass through the slot to handle the newly equipped item. Handles both the removal and/or swapping of the item in the equipment slot
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="index"></param>
    /// <param name="slot"></param>
    private bool Equip<T>(int index, ref T slot) where T : ItemSO
    {
        // first check if valid
        if (!IsIndexValid(index)) return false;
        // type must be a subclass
        if (!typeof(T).IsSubclassOf(typeof(ItemSO))) return false;

        // validate type
        if (items[index].Item is T) {
            T type = items[index].Item as T;

            if(type != null) {
                RemoveItem(index);

                // if there anything in the slot, swap
                if (slot != null) {
                    T temp = slot;
                    slot = type;

                    // insert the item at the previous index
                    items[index].SetItem(temp);
                }
                // if there is nothing in the slot, occupy slot
                else {
                    slot = type;
                }

                return true;
            }
        }

        return false;
    }

    [ContextMenu(nameof(DebugAddItem))]
    private void DebugAddItem()
    {
        int r = UnityEngine.Random.Range(0, testItems.Length);

        AddItem(testItems[r]);
    }

    /// <summary>
    /// Attempts to add an item without going over the maximum inventory size
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private bool TryAddItem(ItemSO item, int stack = 1)
    {
        // find first empty slot
        foreach(Slot slot in items) {
            if(slot.Item == null) {
                slot.SetItem(item);
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Helper method that returns true if the item index is valid and false otherwise
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private bool IsIndexValid(int index)
    {
        if (index < 0 || index > items.Count - 1) return false;
        return true;
    }

    /// <summary>
    /// Safely retrieves an item at the specified index
    /// </summary>
    /// <param name="index"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool TryGetSlot(int index, out Slot slot)
    {
        slot = null;
        if (!IsIndexValid(index)) return false;

        slot = items[index];
        return true;
    }
}
