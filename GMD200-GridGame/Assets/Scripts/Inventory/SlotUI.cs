using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI stackText;

    [SerializeField]
    private Image itemImage;

    private void Awake() => ResetSlot();

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
}
