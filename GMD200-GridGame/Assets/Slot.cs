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
        if (eventData.button != PointerEventData.InputButton.Left) return;

        InventoryItem item = InventoryItem.heldItem;
        if (item == null) return;

        item.transform.SetParent(transform);
        item.Unhold();

        inventory.SetItemPosition(inventory.Items.IndexOf(item.slot), inventory.Items.IndexOf(this));

        item.slot = this;
    }
}
