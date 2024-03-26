using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This item grid can be used for any grid -- inventory, crafting, etc.
/// </summary>
public class ItemGrid : MonoBehaviour
{
    // records the currently held slot, doesnt matter what grid its in
    public static Slot CarriedSlot;

    [SerializeField]
    private Vector2Int dimensions;

    private GenericGrid<Slot> slots;

    // accessors
    public Vector2Int Dimensions { get { return dimensions; } }
    public GenericGrid<Slot> Slots { get { return slots; } }

    public delegate void ItemAdded(ItemSO item);
    public ItemAdded OnItemAdded;

    private void Awake()
    {
        // initialize
        slots = new GenericGrid<Slot>(dimensions.x, dimensions.y, (GenericGrid<Slot> g, int x, int y) => new Slot(g, x, y));
    }

    public bool AddItem(ItemSO item)
    {
        // search for empty item
        if (item.Stack.CanStack)
        {
            for (int x = 0; x < dimensions.x; x++)
            {
                for (int y = 0; y < dimensions.y; y++)
                {
                    Slot slot = slots.GetGridObject(x, y);

                    // null? set item
                    if (slot.Item == null) continue;

                    // try add to stack
                    else if (slot.Item.ItemName == item.ItemName && slot.AddToStack())
                    {
                        OnItemAdded?.Invoke(item);
                        return true;
                    }
                }
            }
        }

        // attempt to add item anywhere
        if (TryAddItem(item))
        {
            return true;
        }
        return false;
    }

    public bool CheckForItem(ItemSO item)
    {
        for (int x = 0; x < dimensions.x; x++)
        {
            for (int y = 0; y < dimensions.y; y++)
            {
                Slot slot = slots.GetGridObject(x, y);

                if (slot.Item != null && item != null && slot.Item.ItemName == item.ItemName) return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Removes item at the specified position, either entirely wiped or removed from the stack.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="removeEntirely"></param>
    public void RemoveItemAtPosition(int x, int y)
    {
        Slot slot = slots.GetGridObject(x, y);

        // if already removed, dont bother
        if (slot.Item == null) return;
        // remove from stack
        slot.RemoveFromStack();
    }

    public void DropFromSlot(int x, int y)
    {
        Slot slot = slots.GetGridObject(x, y);

        // if already removed, dont bother
        if (slot.Item == null) return;

        // create drop
        ItemDrop drop = Instantiate(GameAssets.Instance.ItemDropPrefab, GameManager.Instance.player.transform.position, Quaternion.identity);
        drop.Init(GameManager.Instance.player.transform, slot.Item, 1);

        // remove from stack
        slot.RemoveFromStack(1);
    }

    /// <summary>
    /// Moves the contents of a slot to a new grid
    /// </summary>
    /// <param name="grid"></param>
    /// <param name="index"></param>
    /// <param name="newIndex"></param>
    public bool MoveSlotContentsToGrid(GenericGrid<Slot> newGrid, Vector2Int index, Vector2Int newIndex)
    {
        Slot oldSlot = slots.GetGridObject(index.x, index.y);
        Slot newSlot = newGrid.GetGridObject(newIndex.x, newIndex.y);

        if (oldSlot == null || newSlot == null) return false;
        if (newSlot.Item != null) return false;

        // set the new slot item
        newSlot.SetItem(oldSlot.Item, oldSlot.Stack);

        // reset the old slot
        ResetSlotAtPosition(index.x, index.y);

        return true;
    }

    /// <summary>
    /// Resets a slot at the provided x and y position on the grid
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void ResetSlotAtPosition(int x, int y)
    {
        Slot slot = slots.GetGridObject(x, y);

        // if already removed, dont bother
        if (slot.Item == null) return;

        slot.ResetSlot();
    }

    public bool AddItemAtPosition(ItemSO item, int x, int y)
    {
        Slot slot = slots.GetGridObject(x, y);

        // attempt to add item at empty slot
        if (slot.Item == null)
        {
            slot.SetItem(item);
            return true;
        }
        // stack item as long as they match
        else if (slot.Item.ItemName == item.ItemName && slot.AddToStack()) return true;

        // failed to add item at position
        return false;
    }

    private bool TryAddItem(ItemSO item)
    {
        // find first occourance of a null item
        for (int x = 0; x < dimensions.y; x++)
        {
            for (int y = 0; y < dimensions.x; y++)
            {
                // ???
                Slot slot = slots.GetGridObject(y, x);

                if (slot.Item == null)
                {
                    slot.SetItem(item);
                    OnItemAdded?.Invoke(item);
                    return true;
                }
            }
        }

        return false;
    }

    public Slot GetSlot(int x, int y) => slots.GetGridObject(x, y);

    /// <summary>
    /// Handles gathering all of a specified type into the carried slot. Gathers all into a list and prioritizes the stacks that are fewer
    /// </summary>
    public void GatherAllIntoCarried()
    {
        // Must be existant
        if (CarriedSlot == null || CarriedSlot.Item == null) return;

        List<Slot> matchingTypes = new List<Slot>();

        // iterate through inventory
        for (int x = 0; x < dimensions.x; x++)
        {
            for (int y = 0; y < dimensions.y; y++)
            {
                // ensure does not surpass the stack max
                if (CarriedSlot.Stack >= CarriedSlot.MaxStack) return;
                Slot s = slots.GetGridObject(x, y);

                // ensure items match
                if (s.Item != null && s.Item.ItemName == CarriedSlot.Item.ItemName)
                {
                    // add to matching types
                    matchingTypes.Add(s);
                }
            }
        }

        // Order by the stack amount to ensure smaller stacks are grabbed first before larger stacks
        foreach (Slot s in matchingTypes.OrderBy(x => x.Stack))
        {
            int stack = s.Stack;
            int remainder = (stack + CarriedSlot.Stack) - CarriedSlot.MaxStack;
            if (remainder < 0) remainder = 0;

            for (int i = 0; i < stack - remainder; i++)
            {
                s.RemoveFromStack();
                CarriedSlot.AddToStack();
            }
        }
    }

    /// <summary>
    /// Sets the static carried slot to the contents of the provided slot, resetting the provided slot.
    /// </summary>
    /// <param name="slot"></param>
    public bool SetCarriedSlot(Slot slot)
    {
        if (CarriedSlot != null) return false;

        // set slot
        CarriedSlot = new Slot(slot.Grid, slot.x, slot.y);
        CarriedSlot.SetItem(slot.Item, slot.Stack);

        // reset slot entirely
        ResetSlotAtPosition(slot.x, slot.y);
        return true;
    }

    /// <summary>
    /// Swaps two items from the grid into the carried slot
    /// </summary>
    /// <param name="slot"></param>
    public bool SwapIntoCarried(Slot slot)
    {
        if (ItemGrid.CarriedSlot.Item != null && slot.Item.ItemName == ItemGrid.CarriedSlot.Item.ItemName) return false;

        Slot temp = new Slot(slot.Grid, slot.x, slot.y);
        temp.SetItem(slot.Item, slot.Stack);

        slot.SetItem(CarriedSlot.Item, CarriedSlot.Stack);
        CarriedSlot.SetItem(temp.Item, temp.Stack);

        return true;
    }

    /// <summary>
    /// Takes one from carried slot and places into the provided slot
    /// </summary>
    /// <param name="slot"></param>
    public void PlaceOneFromCarried(Slot slot)
    {
        if (slot.Item != null && (slot.Item.ItemName != ItemGrid.CarriedSlot.Item.ItemName || slot.Item == null || slot.Stack >= slot.MaxStack)) return;

        AddItemAtPosition(ItemGrid.CarriedSlot.Item, slot.x, slot.y);
        CarriedSlot.RemoveFromStack();

        // place down entirely
        if (CarriedSlot.Stack == 0) CarriedSlot = null;
    }

    /// <summary>
    /// Splits provided slot in (roughly) half and puts the other, greater half into the carried slot
    /// </summary>
    /// <param name="slot"></param>
    public void SplitSlotIntoCarried(Slot slot)
    {
        CarriedSlot = new Slot(slots, -1, -1);

        int resultA = (slot.Stack / 2) + (slot.Stack % 2);
        int resultB = slot.Stack / 2;

        ItemGrid.CarriedSlot.SetItem(slot.Item, resultA);
        slot.SetItem(slot.Item, resultB);
    }

    public void AddStackIntoCarried(Slot slot)
    {
        int stack = CarriedSlot.Stack;
        int remainder = (stack + slot.Stack) - slot.MaxStack;
        bool overStacked = (stack + slot.Stack) > slot.MaxStack;

        for (int i = 0; i < stack; i++)
        {
            CarriedSlot.RemoveFromStack();
            slot.AddToStack();
        }

        // check for overstacking
        if (overStacked) CarriedSlot.SetItem(slot.Item, remainder);
        else CarriedSlot = null;
    }

    public void AddTillMaxStack(Slot s1, Slot s2)
    {
        int stack = s1.Stack;
        int remainder = (stack + s2.Stack) - s2.MaxStack;
        if (remainder < 0) remainder = 0;

        for (int i = 0; i < stack - remainder; i++)
        {
            s1.RemoveFromStack();
            s2.AddToStack();
        }
    }

    /// <summary>
    /// Drops everything from the inventory
    /// </summary>
    public void DropAll()
    {
        for(int x = 0; x < dimensions.x; x++)
        {
            for(int y = 0; y < dimensions.y; y++)
            {
                Slot slot = slots.GetGridObject(x, y);

                // if already removed, dont bother
                if (slot.Item == null) continue;

                // create drop
                ItemDrop drop = Instantiate(GameAssets.Instance.ItemDropPrefab, GameManager.Instance.player.transform.position, Quaternion.identity);
                drop.Init(GameManager.Instance.player.transform, slot.Item.Clone(), slot.Stack);

                slot.ResetSlot();
            }
        }
    }

    /// <summary>
    /// Drops all from the carried slot onto the ground as item drops
    /// </summary>
    public static void DropCarried()
    {
        if (ItemGrid.CarriedSlot == null || ItemGrid.CarriedSlot.Item == null) return;

        ItemDrop drop = Instantiate(GameAssets.Instance.ItemDropPrefab, GameManager.Instance.player.transform.position, Quaternion.identity);
        drop.Init(GameManager.Instance.player.transform, ItemGrid.CarriedSlot.Item, ItemGrid.CarriedSlot.Stack);

        ItemGrid.CarriedSlot.ResetSlot();
        ItemGrid.CarriedSlot = null;
    }

    /// <summary>
    /// Drops one from the carried slot
    /// </summary>
    public static void DropOneFromCarried()
    {
        if (ItemGrid.CarriedSlot == null || ItemGrid.CarriedSlot.Item == null) return;

        ItemDrop drop = Instantiate(GameAssets.Instance.ItemDropPrefab, GameManager.Instance.player.transform.position, Quaternion.identity);
        drop.Init(GameManager.Instance.player.transform, ItemGrid.CarriedSlot.Item, 1);

        // reset stack
        if (ItemGrid.CarriedSlot.Stack - 1 == 0)
        {
            ItemGrid.CarriedSlot.ResetSlot();
            ItemGrid.CarriedSlot = null;
        }
        // remove 1
        else
        {
            ItemGrid.CarriedSlot.RemoveFromStack(1);
        }
    }

    /// <summary>
    /// Checks the entire inventory, searching for if there inventory is completely full and theres no way this item will fit.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool IsFullOfItem(ItemSO item)
    {
        // iterate through inventory
        for (int x = 0; x < dimensions.x; x++)
        {
            for (int y = 0; y < dimensions.y; y++)
            {
                Slot slot = slots.GetGridObject(x, y);

                // empty slot detected or slot not full enough
                if (slot.Item == null || (slot.Item == item && slot.Stack < slot.MaxStack)) return false;
            }
        }

        return true;
    }
}

[System.Serializable]
/// <summary>
/// Handles all necessary functions for slots
/// </summary>
public class Slot
{
    // grid references
    private GenericGrid<Slot> grid;
    private int _x, _y;

    // item variables
    private ItemSO item;
    private int stack;

    // accessors
    public GenericGrid<Slot> Grid { get { return grid; } }
    public int Stack { get { return stack; } }
    public ItemSO Item { get { return item; } }
    public int MaxStack { get { return item.Stack.CanStack ? item.Stack.MaxStack : 1; } }
    public int x { get { return _x; } }
    public int y { get { return _y; } }

    public Slot(GenericGrid<Slot> grid, int x, int y)
    {
        // set values
        this.grid = grid;
        _x = x;
        _y = y;
        stack = 0;

        grid.TriggerGridObjectChanged(_x, _y);
    }

    public void SetItem(ItemSO item, int stack = 1)
    {
        if (item != null) this.item = item.Clone();
        else this.item = null;

        this.stack = stack;

        grid.TriggerGridObjectChanged(_x, _y);
    }

    public bool AddToStack(int amount = 1)
    {
        if (stack >= MaxStack) return false;

        stack += amount;
        grid.TriggerGridObjectChanged(_x, _y);

        return true;
    }

    public void RemoveFromStack(int amount = 1)
    {
        stack -= amount;

        if (stack == 0)
        {
            item = null;
        }

        grid.TriggerGridObjectChanged(_x, _y);
    }

    public void ResetSlot()
    {
        item = null;
        stack = 0;

        grid.TriggerGridObjectChanged(_x, _y);
    }
}
