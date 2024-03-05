using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public ItemSO[] testItem;

    [SerializeField]
    private Vector2Int dimensions;

    private GenericGrid<Slot> slots;

    private void Start()
    {
        // initialize
        slots = new GenericGrid<Slot>(dimensions.x, dimensions.y, (GenericGrid<Slot> g, int x, int y) => new Slot(g, x ,y));
        slots.OnGridObjectChanged += PrintGrid;

        AddItem(testItem[UnityEngine.Random.Range(0, testItem.Length - 1)]);
        AddItem(testItem[UnityEngine.Random.Range(0, testItem.Length - 1)]);
        AddItem(testItem[UnityEngine.Random.Range(0, testItem.Length - 1)]);
        AddItem(testItem[UnityEngine.Random.Range(0, testItem.Length - 1)]);

        RemoveItemAtPosition(1, 0, false);
    }

    private void PrintGrid(object sender, GenericGrid<Slot>.OnGridObjectChangedArgs e)
    {
        Slot s = slots.GetGridObject(e.x, e.y);

        Debug.Log($"Updated: [{e.x}, {e.y}] to {s.Item} of stack {s.Stack}");
    }

    public bool AddItem(ItemSO item)
    {
        // search for empty item
        if (item.Stack.CanStack)
        {
            for (int x = 0; x < dimensions.y; x++)
            {
                for (int y = 0; y < dimensions.x; y++)
                {
                    Slot slot = slots.GetGridObject(y, x);

                    // null? set item
                    if (slot.Item == null) continue;

                    // try add to stack
                    else if (slot.Item.ItemName == item.ItemName && slot.AddToStack()) return true;
                }
            }
        }

        // attempt to add item anywhere
        if (TryAddItem(item)) return true;

        return false;
    }

    /// <summary>
    /// Removes item at the specified position, either entirely wiped or removed from the stack.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="removeEntirely"></param>
    private void RemoveItemAtPosition(int x, int y, bool removeEntirely)
    {
        Slot slot = slots.GetGridObject(x, y);

        // if already removed, dont bother
        if (slot.Item == null) return;

        // reset both item and stack
        if (removeEntirely) slot.ResetSlot();
        // remove from stack
        else slot.RemoveFromStack();
    }

    public bool AddItemAtPosition(ItemSO item, int x, int y)
    {
        Slot slot = slots.GetGridObject(x, y);

        // attempt to add item at empty slot
        if(slot.Item == null) {
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
                Slot slot = slots.GetGridObject(y, x);

                if(slot.Item == null) {
                    slot.SetItem(item);
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Swaps two items at the provided indexes
    /// </summary>
    /// <param name="i"></param>
    /// <param name="j"></param>
    public void SwapItems(Vector2Int i, Vector2Int j)
    {
        // get corresponding slots
        Slot s1 = slots.GetGridObject(i.x, i.y);    
        Slot s2 = slots.GetGridObject(j.x, j.y);

        // ensure both exist
        if (s1 == null || s2 == null) return;
        Slot temp = s1;

        // perform swap
        s1.SetItem(s2.Item, s2.Stack);
        s2.SetItem(temp.Item, temp.Stack);
    }
}

public class Slot
{
    // grid references
    private GenericGrid<Slot> grid;
    private int x, y;

    // item variables
    private ItemSO item;
    private int stack;

    // accessors
    public int Stack { get { return stack; } }
    public ItemSO Item { get { return item; } }
    public int MaxStack { get { return item.Stack.CanStack ? item.Stack.MaxStack : 1; } }

    public Slot(GenericGrid<Slot> grid, int x, int y)
    {
        // set values
        this.grid = grid;
        this.x = x;
        this.y = y;
        stack = 0;

        grid.TriggerGridObjectChanged(x, y);
    }

    public void SetItem(ItemSO item, int stack = 1)
    {
        this.item = item.Clone();
        this.stack = stack;

        grid.TriggerGridObjectChanged(x, y);
    }

    public bool AddToStack(int amount = 1)
    {
        if (stack >= MaxStack) return false;

        stack += amount;
        grid.TriggerGridObjectChanged(x, y);

        return true;
    }

    public void RemoveFromStack(int amount = 1)
    {
        stack -= amount;

        if(stack == 0) {
            item = null;
        }

        grid.TriggerGridObjectChanged(x, y);
    }

    public void ResetSlot()
    {
        item = null;
        stack = 0;

        grid.TriggerGridObjectChanged(x, y);
    }
}
