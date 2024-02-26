using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;
using UnityEditorInternal.Profiling.Memory.Experimental;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private int _maxInventorySize = 16;

    // the list of items
    private List<Slot> items = new List<Slot>();

    // equiped items. equipped items are not recorded in the item list
    private ArmorSO equippedArmor;
    private WeaponSO equippedWeapon;

    // accessors
    public ArmorSO EquippedArmor { get { return equippedArmor; } }
    public WeaponSO EquippedWeapon { get { return equippedWeapon; } }
    public List<Slot> Items { get { return items; } }

    // delegates
    public delegate void ArmorChanged(ArmorSO oldArmor, ArmorSO newArmor);
    public delegate void WeaponChanged(WeaponSO oldWeapon, WeaponSO newWeapon);
    public delegate void ItemAdded(ItemSO item);
    public delegate void ItemRemoved(ItemSO item);

    public ArmorChanged OnArmorChanged;
    public WeaponChanged OnWeaponChanged;
    public ItemAdded OnItemAdded;
    public ItemRemoved OnItemRemoved;

    /// <summary>
    /// Returns true if the item was successfully added, returns false if the item could not be added.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool AddItem(ItemSO item)
    {
        // cannot be a null call
        if (item == null) return false;

        // if the item can stack, check to see if theres a matching item
        if (item.Stack.CanStack) {
            bool found = false;

            // iterate through each item and check if the items match
            foreach (Slot i in items)
            {
                // if items match
                if (i.item.ItemID == item.ItemID) {
                    // cannot exceed the maximum stack
                    if (i.count >= i.item.Stack.MaxStack) continue;

                    // increase the stack if one was found
                    i.count++;

                    found = true;
                    break;
                }
            }

            // try to add the item without stacking
            if (!found)
                if (!TryAddItem(new Slot(item, 1)))
                    return false;
        }
        else {
            // try to add the item without stacking
            if (!TryAddItem(new Slot(item, 1))) return false;
        }

        // invoke the successful item add event and return true
        OnItemAdded?.Invoke(item);
        return true;
    }

    /// <summary>
    /// Removes an item from the inventory
    /// </summary>
    /// <param name="item"></param>
    public void RemoveItem(int index)
    {
        if (!IsIndexValid(index)) return;

        Slot item = items[index];

        // if the item is stacked, decrease the count
        if (item.count > 1)
        {
            item.count--;

            // call the remove event
            OnItemRemoved?.Invoke(item.item);
        }
        // otherwise, remove the item at the index
        else
        {
            ItemSO itemSO = items[index].item;

            // remove item and call event
            items.RemoveAt(index);
            OnItemRemoved?.Invoke(itemSO);
        }
    }

    /// <summary>
    /// Equips weapon at corresponding index
    /// </summary>
    /// <param name="index"></param>
    public void EquipWeapon(int index) => Equip<WeaponSO>(index, ref equippedWeapon);

    /// <summary>
    /// Equips armor at the corresponding index
    /// </summary>
    /// <param name="index"></param>
    public void EquipArmor(int index) => Equip<ArmorSO>(index, ref equippedArmor);

    /// <summary>
    /// You pass through the slot to handle the newly equipped item. Handles both the removal and/or swapping of the item in the equipment slot
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="index"></param>
    /// <param name="slot"></param>
    private void Equip<T>(int index, ref T slot) where T : ItemSO
    {
        // first check if valid
        if (!IsIndexValid(index)) return;
        // type must be a subclass
        if (!typeof(T).IsSubclassOf(typeof(ItemSO))) return;

        // validate type
        if (items[index].item is T) {
            T type = items[index].item as T;

            if(type != null) {
                items.RemoveAt(index);

                // if there anything in the slot, swap
                if(slot != null) {
                    T temp = slot;
                    slot = type;

                    // insert the item at the previous index
                    items.Insert(index, new Slot(temp, 1));
                }
                // if there is nothing in the slot, occupy slot
                else {
                    slot = type;
                }
            }
        }
    }

    /// <summary>
    /// Attempts to add an item without going over the maximum inventory size
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private bool TryAddItem(Slot item)
    {
        if (items.Count >= _maxInventorySize) return false;

        items.Add(item);
        return true;
    }

    /// <summary>
    /// Helper method that returns true if the item index is valid and false otherwise
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private bool IsIndexValid(int index)
    {
        if (index < 0 || index > _maxInventorySize - 1) return false;
        return true;
    }
}

/// <summary>
/// Stores the data for each item slot
/// </summary>
///
[System.Serializable]
public class Slot
{
    public ItemSO item;
    public int count;

    // constructor for an item
    public Slot(ItemSO item, int count)
    {
        this.item = item;
        this.count = count;
    }
}
