using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerClickHandler
{
    public static InventoryItem heldItem;
        
    [Header("UI")]
    [SerializeField]
    private Image itemSprite;

    [SerializeField]
    private TextMeshProUGUI itemCountText;

    [HideInInspector]
    public Transform parentAfterDrag;
    [HideInInspector]
    public Slot slot;
    [HideInInspector]
    public bool holding;

    private void Update()
    {
        if (holding) {
            transform.position = Input.mousePosition;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left || heldItem != null) {
            slot.OnPointerClick(eventData);
            return;
        }

        holding = true;

        itemSprite.raycastTarget = false;
        transform.SetParent(transform.root);

        heldItem = this;
    }

    public void Unhold()
    {
        holding = false;
        itemSprite.raycastTarget = true;
        heldItem = null;
    }

    public void UpdateStackCount(int stack)
    {
        if (stack <= 1) itemCountText.gameObject.SetActive(false);
        else itemCountText.gameObject.SetActive(true);

        itemCountText.text = $"{stack}";
    }

    public void UpdateSprite(Sprite sprite) => itemSprite.sprite = sprite;

    public void SetRaycastTarget(bool target) => itemSprite.raycastTarget = target;
}
