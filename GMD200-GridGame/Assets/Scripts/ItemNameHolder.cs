using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ItemNameHolder : MonoBehaviour
{
    [SerializeField]
    private RectTransform holder;
    [SerializeField]
    private TextMeshProUGUI text;

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) {

            if(SlotUI.hoveredUI != null && SlotUI.hoveredUI.MySlot.Item != null) {
                text.SetText(SlotUI.hoveredUI.MySlot.Item.ItemName);
                transform.position = Input.mousePosition + new Vector3(holder.rect.width / 2, holder.rect.height / 2);

                holder.gameObject.SetActive(true);

                return;
            }
        }

        holder.gameObject.SetActive(false);
    }
}
