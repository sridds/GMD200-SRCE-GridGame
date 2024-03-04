using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    [Header("References")]
    [SerializeField]
    private InventoryItem itemPrefab;

    private ItemSO item;
    private InventoryItem myInventoryItem;
    private Inventory inventory;
    private int stack;

    // getter for item
    public ItemSO Item { get { return item; } }
    public int Stack { get { return stack; } }

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    public void Init(ItemSO item, int stack = 1)
    {
        this.item = item.Clone();
        this.stack = stack;

        // instantiate
        if (myInventoryItem == null)
        {
            if(transform.childCount > 0) myInventoryItem = transform.GetChild(0).GetComponent<InventoryItem>();
            else myInventoryItem = Instantiate(itemPrefab, transform);
        }

        myInventoryItem.slot = this;
        myInventoryItem.UpdateSprite(item.ItemSprite);
        myInventoryItem.UpdateStackCount(this.stack);
    }

    public void SetItem(ItemSO item, int stack = 1)
    {
        this.item = item;
        this.stack = stack;

        if (myInventoryItem == null)
        {
            if (transform.childCount > 0) myInventoryItem = transform.GetChild(0).GetComponent<InventoryItem>();
        }
    }

    public void ResetSlot()
    {
        item = null;
        stack = 0;
        myInventoryItem = null;
    }

    public void AddToStack(int amount = 1)
    {
        stack += amount;
        myInventoryItem.UpdateStackCount(this.stack);
    }

    public void RemoveFromStack(int amount = 1)
    {
        stack -= amount;
        myInventoryItem.UpdateStackCount(this.stack);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        InventoryItem heldItem = InventoryItem.heldItem;
        if (heldItem == null) return;

        // place item
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Slot s = inventory.Items[inventory.Items.IndexOf(heldItem.slot)];

            //if(this.item == null) {
                heldItem.transform.SetParent(transform);
                heldItem.Unhold();
                inventory.SetItemPosition(inventory.Items.IndexOf(heldItem.slot), inventory.Items.IndexOf(this));
                heldItem.slot = this;
            //}
            /*
            else {
                // do they not match
                if(this.item.name != heldItem.name) {
                    inventory.SwapSlots(inventory.Items.IndexOf(s), inventory.Items.IndexOf(this));
                }
                /*
                else {
                    Debug.Log("hey listen");
                    stack += s.stack;
                    heldItem.Unhold();
                    s.ResetSlot();
                }
            }*/
        }
        // split item
        else if(eventData.button == PointerEventData.InputButton.Right)
        {
            Slot s = inventory.Items[inventory.Items.IndexOf(heldItem.slot)];

            // ensure enoguh in stack
            if (s.stack > 1) {
                if(myInventoryItem != null) myInventoryItem.SetRaycastTarget(false);

                if (inventory.AddItemAtPosition(s.item, inventory.Items.IndexOf(this))) {
                    s.RemoveFromStack();
                }

                if (myInventoryItem != null) myInventoryItem.SetRaycastTarget(true);
            }
        }
    }
}
