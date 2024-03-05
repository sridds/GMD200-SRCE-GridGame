using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    private ItemGrid myItemGrid;

    [SerializeField]
    private ItemSO[] testItem;

    [SerializeField]
    private List<SlotUI> mySlots;

    private void Start()
    {
        myItemGrid.Slots.OnGridObjectChanged += UpdateSlot;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            myItemGrid.AddItem(testItem[Random.Range(0, testItem.Length - 1)]);
        }
    }

    private void UpdateSlot(object sender, GenericGrid<Slot>.OnGridObjectChangedArgs e)
    {
        Slot s = myItemGrid.Slots.GetGridObject(e.x, e.y);

        mySlots[e.y * myItemGrid.Dimensions.x + e.x].UpdateValues(s.Item.ItemSprite, s.Stack);
    }
}
