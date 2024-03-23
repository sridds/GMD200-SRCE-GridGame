using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CursorItemHandler : MonoBehaviour
{
    [SerializeField]
    private Image itemImage;
    [SerializeField]
    private TextMeshProUGUI stackText;

    void Update()
    {
        if(ItemGrid.CarriedSlot == null) {
            itemImage.enabled = false;
            stackText.text = "";
        }
        else {
            itemImage.enabled = true;

            // mock sprite and text
            itemImage.sprite = ItemGrid.CarriedSlot.Item.ItemSprite;
            stackText.text = ItemGrid.CarriedSlot.Stack > 1 ? $"{ItemGrid.CarriedSlot.Stack}" : "";

            transform.position = Input.mousePosition;
        }
    }
}
