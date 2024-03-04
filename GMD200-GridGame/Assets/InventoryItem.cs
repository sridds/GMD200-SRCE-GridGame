using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI")]
    [SerializeField]
    private Image itemSprite;

    [SerializeField]
    private TextMeshProUGUI itemCountText;

    [HideInInspector]
    public Transform parentAfterDrag;

    public void OnBeginDrag(PointerEventData eventData) {
        itemSprite.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData) {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData) {
        itemSprite.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
    }

    public void UpdateStackCount(int stack)
    {
        if (stack <= 1) itemCountText.gameObject.SetActive(false);
        else itemCountText.gameObject.SetActive(true);

        itemCountText.text = $"{stack}";
    }

    public void UpdateSprite(Sprite sprite) => itemSprite.sprite = sprite;
}
