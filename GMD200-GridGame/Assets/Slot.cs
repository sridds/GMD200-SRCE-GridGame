using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    [Header("References")]
    [SerializeField]
    private InventoryItem itemPrefab;

    private ItemSO item;
    private InventoryItem myInventoryItem;
    private int stack;

    // getter for item
    public ItemSO Item { get { return item; } }
    public int Stack { get { return stack; } }

    public void Init(ItemSO item, int stack = 1)
    {
        this.item = item.Clone();
        this.stack = stack;

        // instantiate
        if(myInventoryItem == null) myInventoryItem = Instantiate(itemPrefab, transform);

        myInventoryItem.UpdateSprite(item.ItemSprite);
        myInventoryItem.UpdateStackCount(this.stack);
    }

    public void SetItem(ItemSO item, int stack = 1)
    {
        this.item = item;
        this.stack = stack;
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

    public void OnDrop(PointerEventData eventData)
    {
        if (myInventoryItem != null) return;

        InventoryItem item = eventData.pointerDrag.GetComponent<InventoryItem>();
        item.parentAfterDrag = transform;
    }
}
