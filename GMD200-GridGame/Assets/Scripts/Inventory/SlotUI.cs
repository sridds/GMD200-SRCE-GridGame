using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Linq;

public class SlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private TextMeshProUGUI stackText;

    [SerializeField]
    private Image itemImage;

    [SerializeField]
    private Slider durabilitySlider;

    [SerializeField]
    private Image durabilityFill;

    [SerializeField]
    private Gradient durabilityGradient;

    [SerializeField]
    private bool takeOnlySlot;

    private ItemGrid myItemGrid;
    private Slot mySlot;

    public delegate void SlotTaken();
    public SlotTaken OnSlotTaken;

    private float lastClickTimestamp;

    public Slot MySlot { get => mySlot; }

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
    }

    private void Update()
    {
        UpdateItem();

        if (mySlot.Item == null) return;
        UpdateDurability();
    }


    int lastStack;
    ItemSO lastItem;

    private void UpdateItem()
    {
        if (mySlot.Item == null)
        {
            lastItem = null;

            ResetSlot();
            return;
        }

        if ((lastItem == null && mySlot.Item != null) || lastItem.name != mySlot.Item.name) lastItem = mySlot.Item;
        else if (lastStack != mySlot.Stack) lastStack = mySlot.Stack;
        else return;

        itemImage.enabled = true;
        itemImage.sprite = mySlot.Item.ItemSprite;
        stackText.text = mySlot.Stack > 1 ? $"{mySlot.Stack}" : "";

        itemImage.rectTransform.DOKill(true);
        itemImage.rectTransform.DOPunchScale(new Vector3(0.5f, 0.5f, 0), 0.2f, 10, 1.3f);
    }

    private void UpdateDurability()
    {
        if (!mySlot.Item.HasDurability)
        {
            durabilitySlider.gameObject.SetActive(false);
            return;
        }

        if (mySlot.Item.MaxDurability != mySlot.Item.CurrentDurability) durabilitySlider.gameObject.SetActive(true);

        durabilitySlider.maxValue = mySlot.Item.MaxDurability;
        durabilitySlider.value = mySlot.Item.CurrentDurability;

        // evaluate
        durabilityFill.color = durabilityGradient.Evaluate((durabilitySlider.value / durabilitySlider.maxValue));
    }

    private void ResetSlot()
    {
        itemImage.enabled = false;
        if(durabilitySlider != null) durabilitySlider.gameObject.SetActive(false);
        stackText.text = "";
        //mySlot = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            // GRAB ALL
            if(Time.time - lastClickTimestamp <= 0.2f && !takeOnlySlot) {
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
                    myItemGrid.AddStackIntoCarried(mySlot);
                }

                else if(ItemGrid.CarriedSlot.Item != null && mySlot.Item.ItemName == ItemGrid.CarriedSlot.Item.ItemName && takeOnlySlot)
                {
                    if(mySlot.Stack + ItemGrid.CarriedSlot.Stack <= ItemGrid.CarriedSlot.MaxStack)
                    {
                        myItemGrid.AddTillMaxStack(mySlot, ItemGrid.CarriedSlot);

                        OnSlotTaken?.Invoke();
                    }
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoveredUI = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoveredUI == this) hoveredUI = null;
    }

    public static SlotUI hoveredUI;
}
