using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SlotUI : MonoBehaviour, IPointerClickHandler
{
    public static SlotUI carriedSlot;

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

    private void Update() => UpdateCarriedSlot();

    private void UpdateCarriedSlot()
    {
        // update position of carried slot
        if (carriedSlot == this) {
            itemImage.transform.position = Input.mousePosition;
        }
    }

    private void ResetSlot()
    {
        itemImage.enabled = false;
        stackText.text = "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // item must be existing to carry
            if(mySlot.Item != null)
            {
                // set carried slot
                if (carriedSlot == null) {
                    SetCarriedSlot();

                    return;
                }
                else if (carriedSlot != this) {

                    // SWAPS TWO ITEMS OF UNLIKE TYPE
                    if(carriedSlot.mySlot.Item == null || mySlot.Item.ItemName != carriedSlot.mySlot.Item.ItemName) {
                        // swap items
                        myItemGrid.SwapItems(new Vector2Int(carriedSlot.mySlot.x, carriedSlot.mySlot.y), new Vector2Int(mySlot.x, mySlot.y));

                        // set parent
                        itemImage.transform.SetParent(transform);
                    }

                    // ADDS TWO MATCHING STACKS
                    else if(mySlot.Item.ItemName == carriedSlot.mySlot.Item.ItemName) {
                        myItemGrid.AddMatchingStacks(new Vector2Int(carriedSlot.mySlot.x, carriedSlot.mySlot.y), new Vector2Int(mySlot.x, mySlot.y));

                        // set parent
                        itemImage.transform.SetParent(transform);
                    }
                }

                else if(carriedSlot == this) {
                    itemImage.transform.SetParent(transform);
                    carriedSlot = null;
                }
            }

            // PLACES ITEM BACK DOWN
            else if(carriedSlot != null && carriedSlot != this)
            {
                myItemGrid.SwapItems(new Vector2Int(mySlot.x, mySlot.y), new Vector2Int(carriedSlot.mySlot.x, carriedSlot.mySlot.y));

                carriedSlot.itemImage.transform.SetParent(carriedSlot.transform);
                carriedSlot = null;
            }
        }

        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            // PLACE ONE ON ME
            if (carriedSlot != null && carriedSlot != this) {
                // compare items
                if (mySlot.Item == null || mySlot.Item.ItemName == carriedSlot.mySlot.Item.ItemName) {
                    myItemGrid.SwapFromStack(new Vector2Int(carriedSlot.mySlot.x, carriedSlot.mySlot.y), new Vector2Int(mySlot.x, mySlot.y));

                    if(carriedSlot.mySlot.Stack == 0) {
                        carriedSlot.itemImage.transform.SetParent(carriedSlot.transform);
                        carriedSlot = null;
                    }
                }
            }

            // SPLIT STACK
            else if(mySlot.Item == null && carriedSlot == null){

            }
        }
    }

    private void SetCarriedSlot()
    {
        carriedSlot = this;
        itemImage.transform.SetParent(transform.root);
    }
}
