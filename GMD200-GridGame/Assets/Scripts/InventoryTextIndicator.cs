using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryTextIndicator : MonoBehaviour
{
    [SerializeField]
    private InventoryTextBehaviour textIndicatorPrefab;

    private List<InventoryTextBehaviour> activeIndicators = new List<InventoryTextBehaviour>();

    private void Start()
    {
        GameManager.Instance.inventory.OnItemAdded += (ItemSO item) => CreateTextIndicator(item, 1);
    }

    public void CreateTextIndicator(ItemSO item, int count)
    {
        // find matching indicator to iterate
        foreach(InventoryTextBehaviour i in activeIndicators)
        {
            if(i.MyItem.ItemName == item.ItemName) {
                i.SetItem(item, i.Count + count);

                return;
            }
        }

        // instantiate new text and set item
        InventoryTextBehaviour text = Instantiate(textIndicatorPrefab, transform);
        text.SetItem(item, count);

        // cache to list
        activeIndicators.Add(text);
    }

    public void Remove(InventoryTextBehaviour behaviour) => activeIndicators.Remove(behaviour);
}
