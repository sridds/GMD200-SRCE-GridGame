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

    private void Start() => myItemGrid.Slots.OnGridObjectChanged += UpdateSlot;

    private void Update()
    {
        // debug key
        if (Input.GetKeyDown(KeyCode.E)) {
            myItemGrid.AddItem(testItem[Random.Range(0, testItem.Length - 1)]);
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            int x = Random.Range(0, myItemGrid.Dimensions.x);
            int y = Random.Range(0, myItemGrid.Dimensions.y);

            Debug.Log($"Attempting to remove at: [{x},{y}]");
            myItemGrid.RemoveItemAtPosition(x, y);
        }
    }

    /// <summary>
    /// Updates the slot depiction to the corresponding values
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void UpdateSlot(object sender, GenericGrid<Slot>.OnGridObjectChangedArgs e)
    {
        Slot s = myItemGrid.Slots.GetGridObject(e.x, e.y);

        Sprite spr = s.Item == null ? null : s.Item.ItemSprite;

        mySlots[e.y * myItemGrid.Dimensions.x + e.x].UpdateValues(spr, s.Stack);
    }
}
