using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OutsideClickHandler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private ItemDrop itemDropPrefab;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (ItemGrid.CarriedSlot == null || ItemGrid.CarriedSlot.Item == null) return;

        // drop all in cursor
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            ItemDrop drop = Instantiate(itemDropPrefab, GameManager.Instance.player.transform.position, Quaternion.identity);
            drop.Init(GameManager.Instance.player.transform, ItemGrid.CarriedSlot.Item, ItemGrid.CarriedSlot.Stack);
            
            ItemGrid.CarriedSlot.ResetSlot();
            ItemGrid.CarriedSlot = null;
        }

        // drop one outside
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            ItemDrop drop = Instantiate(itemDropPrefab, GameManager.Instance.player.transform.position, Quaternion.identity);
            drop.Init(GameManager.Instance.player.transform, ItemGrid.CarriedSlot.Item, 1);

            // reset stack
            if (ItemGrid.CarriedSlot.Stack - 1 == 0) {
                ItemGrid.CarriedSlot.ResetSlot();
                ItemGrid.CarriedSlot = null;
            }
            // remove 1
            else {
                ItemGrid.CarriedSlot.RemoveFromStack(1);
            }
        }
    }
}
