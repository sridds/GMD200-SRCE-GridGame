using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField]
    private RectTransform inventoryRect;

    private bool inventoryDisplaying;

    private void Update()
    {
        if (GameManager.Instance.currentGameState == GameState.Paused) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        inventoryDisplaying = !inventoryDisplaying;
        inventoryRect.gameObject.SetActive(inventoryDisplaying);

        if (inventoryDisplaying)
        {
            GameManager.Instance.currentGameState = GameState.UI;
        }
        else
        {
            GameManager.Instance.currentGameState = GameState.Playing;
            ItemGrid.DropCarried();
        }
    }
}
