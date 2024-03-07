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

    [SerializeField]
    private bool takeOnlySlot;

    private ItemGrid myItemGrid;
    private Slot mySlot;

    public delegate void SlotTaken();
    public SlotTaken OnSlotTaken;

    private float lastClickTimestamp;

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
            // GRAB ALL
            if(Time.time - lastClickTimestamp <= 0.2f) {
                myItemGrid.GatherAllIntoCarried();
            }

            // PICKUP ITEM
            else if (mySlot.Item != null)
            {
                if(ItemGrid.CarriedSlot == null) {
                    myItemGrid.SetCarriedSlot(mySlot);
                    OnSlotTaken?.Invoke();
                }

                // SWAP SLOTS
                else if (ItemGrid.CarriedSlot.Item == null || mySlot.Item.ItemName != ItemGrid.CarriedSlot.Item.ItemName && !takeOnlySlot)
                {
                    myItemGrid.SwapIntoCarried(mySlot);
                }

                // Add stacks
                else if(ItemGrid.CarriedSlot.Item != null && mySlot.Item.ItemName == ItemGrid.CarriedSlot.Item.ItemName && !takeOnlySlot)
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

                else if(ItemGrid.CarriedSlot.Item != null && mySlot.Item.ItemName == ItemGrid.CarriedSlot.Item.ItemName && takeOnlySlot)
                {
                    int stack = mySlot.Stack;
                    int remainder = (stack + ItemGrid.CarriedSlot.Stack) - ItemGrid.CarriedSlot.MaxStack;
                    if (remainder < 0) remainder = 0;

                    for (int i = 0; i < stack - remainder; i++)
                    {
                        mySlot.RemoveFromStack();
                        ItemGrid.CarriedSlot.AddToStack();
                    }

                    OnSlotTaken?.Invoke();
                }
            }

            // PLACE ITEM DOWN
            else if(ItemGrid.CarriedSlot != null && !takeOnlySlot)
            {
                mySlot.SetItem(ItemGrid.CarriedSlot.Item, ItemGrid.CarriedSlot.Stack);
                ItemGrid.CarriedSlot = null;
            }

            lastClickTimestamp = Time.time;
        }

        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (ItemGrid.CarriedSlot != null && !takeOnlySlot)
            {
                myItemGrid.PlaceOneFromCarried(mySlot);
            }

            // SPLIT STACK
            else if(mySlot.Item != null && mySlot.Stack > 1)
            {
                myItemGrid.SplitSlotIntoCarried(mySlot);
            }
        }
    }
}
