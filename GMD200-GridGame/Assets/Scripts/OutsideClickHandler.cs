using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OutsideClickHandler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private ItemGrid grid;

    public void OnPointerClick(PointerEventData eventData)
    {
        // drop all in cursor
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            ItemGrid.DropCarried();
        }

        // drop one outside
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            ItemGrid.DropOneFromCarried();
        }
    }
}
