using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Linq;

public class SlotUI : MonoBehaviour, IPointerClickHandler
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

        itemImage.rectTransform.DOKill(true);
        itemImage.rectTransform.DOPunchScale(new Vector3(0.5f, 0.5f, 0), 0.2f, 10, 1.3f);

        itemImage.enabled = true;
        itemImage.sprite = spr;
        stackText.text = stackAmt > 1 ? $"{stackAmt}" : "";

        if (mySlot.Item == null) return;

        if (mySlot.Item.Durability.HasDurability && mySlot.Durability != mySlot.Item.Durability.MaxDurability)
        {
            durabilitySlider.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (mySlot.Item == null) return;

        if (!mySlot.Item.Durability.HasDurability) return;

        durabilitySlider.maxValue = mySlot.Item.Durability.MaxDurability;
        durabilitySlider.value = mySlot.Durability;

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
                mySlot.SetItem(ItemGrid.CarriedSlot.Item, ItemGrid.CarriedSlot.Stack, ItemGrid.CarriedSlot.Durability);
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
