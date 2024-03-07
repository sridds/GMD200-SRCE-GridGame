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

    private void Start() {
        myItemGrid.Slots.OnGridObjectChanged += UpdateSlot;

        // initialize each slot w reference to item grid
        for(int i = 0; i < mySlots.Count; i++) {
            mySlots[i].Initialize(myItemGrid.Slots.GetGridObject(i % myItemGrid.Dimensions.x, i / myItemGrid.Dimensions.x), myItemGrid);
        }
    }

    private void Update()
    {
        // debug key
        if (Input.GetKeyDown(KeyCode.E) && testItem.Length > 0) {
            myItemGrid.AddItem(testItem[Random.Range(0, testItem.Length)]);
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

        // ensure the sprite doesnt throw a null reference exception when trying to access a null item
        Sprite spr = s.Item == null ? null : s.Item.ItemSprite;
        mySlots[e.y * myItemGrid.Dimensions.x + e.x].UpdateValues(spr, s.Stack);
    }
}
