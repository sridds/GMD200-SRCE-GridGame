using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SlotUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private TextMeshProUGUI stackText;

    [SerializeField]
    private Image itemImage;

    private ItemGrid myItemGrid;
    private Slot mySlot;

    private void Awake() => ResetSlot();

    /// <summary>
    /// initializes the slot with the necessary variables
    /// </summary>
    /// <param name="mySlot"></param>
    /// <param name="myItemGrid"></param>
    public void Initialize(Slot mySlot, ItemGrid myItemGrid)
    {
        this.mySlot = mySlot;
        this.myItemGrid = myItemGrid;
    }

    /// <summary>
    /// Updates values to the corresponding sprite and stack
    /// </summary>
    /// <param name="spr"></param>
    /// <param name="stackAmt"></param>
    public void UpdateValues(Sprite spr, int stackAmt)
    {
        if(stackAmt == 0) {
            ResetSlot();
            return;
        }

        itemImage.enabled = true;
        itemImage.sprite = spr;
        stackText.text = stackAmt > 1 ? $"{stackAmt}" : "";
    }

    private void ResetSlot()
    {
        itemImage.enabled = false;
        stackText.text = "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            // PICKUP ITEM
            if(mySlot.Item != null)
            {
                if(ItemGrid.CarriedSlot == null)
                {
                    // set slot
                    ItemGrid.CarriedSlot = new Slot(mySlot.Grid, mySlot.x, mySlot.y);
                    ItemGrid.CarriedSlot.SetItem(mySlot.Item, mySlot.Stack);

                    // reset slot entirely
                    myItemGrid.ResetSlotAtPosition(mySlot.x, mySlot.y);
                }

                // SWAP SLOTS
                else if (ItemGrid.CarriedSlot.Item == null || mySlot.Item.ItemName != ItemGrid.CarriedSlot.Item.ItemName)
                {
                    Slot temp = new Slot(mySlot.Grid, mySlot.x, mySlot.y);
                    temp.SetItem(mySlot.Item, mySlot.Stack);

                    mySlot.SetItem(ItemGrid.CarriedSlot.Item, ItemGrid.CarriedSlot.Stack);
                    ItemGrid.CarriedSlot.SetItem(temp.Item, temp.Stack);
                }

                // Add stacks
                else if(ItemGrid.CarriedSlot.Item != null && mySlot.Item.ItemName == ItemGrid.CarriedSlot.Item.ItemName)
                {
                    int stack = ItemGrid.CarriedSlot.Stack;
                    int remainder = (stack + mySlot.Stack) - mySlot.MaxStack;
                    bool overStacked = (stack + mySlot.Stack) > mySlot.MaxStack;

                    for(int i = 0; i < stack; i++) {
                        ItemGrid.CarriedSlot.RemoveFromStack();
                        mySlot.AddToStack();
                    }

                    // check for overstacking
                    if (overStacked) ItemGrid.CarriedSlot.SetItem(mySlot.Item, remainder);
                    else ItemGrid.CarriedSlot = null;
                }
            }

            // PLACE ITEM DOWN
            else if(ItemGrid.CarriedSlot != null){
                mySlot.SetItem(ItemGrid.CarriedSlot.Item, ItemGrid.CarriedSlot.Stack);
                ItemGrid.CarriedSlot = null;
            }
        }

        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (ItemGrid.CarriedSlot != null)
            {
                // PLACE ONE
                if (mySlot.Item == null || (mySlot.Item.ItemName == ItemGrid.CarriedSlot.Item.ItemName && mySlot.Item != null && mySlot.Stack < mySlot.MaxStack)) {
                    myItemGrid.AddItemAtPosition(ItemGrid.CarriedSlot.Item, mySlot.x, mySlot.y);
                    ItemGrid.CarriedSlot.RemoveFromStack();

                    // place down entirely
                    if (ItemGrid.CarriedSlot.Stack == 0) ItemGrid.CarriedSlot = null;
                }
            }

            // SPLIT STACK
            else if(mySlot.Item != null && mySlot.Stack > 1){
                ItemGrid.CarriedSlot = new Slot(myItemGrid.Slots, -1, -1);

                int resultA = (mySlot.Stack / 2) + (mySlot.Stack % 2);
                int resultB = mySlot.Stack / 2;

                ItemGrid.CarriedSlot.SetItem(mySlot.Item, resultB);
                mySlot.SetItem(mySlot.Item, resultA);
            }
        }
    }
}
