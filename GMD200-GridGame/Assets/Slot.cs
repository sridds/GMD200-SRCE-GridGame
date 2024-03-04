using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Image itemSprite;

    [SerializeField]
    private TextMeshProUGUI itemCountText;

    private ItemSO item;
    private int stack;

    // getter for item
    public ItemSO Item { get { return item; } }
    public int Stack { get { return stack; } }

    public void SetItem(ItemSO item, int stack = 1)
    {
        this.item = item.Clone();
        this.stack = stack;

        // setup sprite
        itemSprite.gameObject.SetActive(true);
        itemSprite.sprite = item.ItemSprite;

        if(this.stack > 1) {
            itemCountText.gameObject.SetActive(true);
            itemCountText.text = $"{this.stack}";
        }
        else {
            itemCountText.gameObject.SetActive(false);
        }
    }

    public void AddToStack(int amount = 1)
    {
        stack += amount;

        if (this.stack > 1) {
            itemCountText.gameObject.SetActive(true);
            itemCountText.text = $"{this.stack}";
        }
    }
    public void RemoveFromStack(int amount = 1)
    {
        stack -= amount;
        itemCountText.text = $"{this.stack}";

        if (stack <= 0)
        {
            item = null;

            // reset sprite
            itemSprite.gameObject.SetActive(false);
            itemSprite.sprite = null;
        }
        else if (stack < 1) {
            itemCountText.gameObject.SetActive(false);
            itemCountText.text = "";
        }
    }
}
