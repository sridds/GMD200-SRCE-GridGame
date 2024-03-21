using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField]
    private RectTransform inventoryRect;

    public int CurrentSlot { get; private set; }
    private bool inventoryDisplaying;

    public delegate void CurrentSlotUpdate(int slot);
    public CurrentSlotUpdate OnUpdateCurrentSlot;

    private void Update()
    {
        if (GameManager.Instance.currentGameState == GameState.Paused) return;

        UpdateCurrentSlot(); // update current slot input

        // toggles the inventory on and off
        if (Input.GetKeyDown(KeyCode.E)) ToggleInventory();
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

            // anything that is currently carried should be dropped
            ItemGrid.DropCarried();
        }
    }

    /// <summary>
    /// Gets scroll input and updates the current slot index accordingly
    /// </summary>
    private void UpdateCurrentSlot()
    {
        if (GameManager.Instance.currentGameState == GameState.UI) return;

        // get scroll value of player
        float scrollValue = Input.mouseScrollDelta.y;

        // get sign of value. if 0, return. no change
        int val = (int)Mathf.Sign(scrollValue);
        if (scrollValue == 0) return;

        // assumes the hotbar is
        int length = GameManager.Instance.inventory.Dimensions.x;
        int previousWeaponIndex = CurrentSlot;

        // add value to current slot, ensure it doesnt go out of range
        CurrentSlot += val;
        CurrentSlot = (CurrentSlot % length + length) % length;

        OnUpdateCurrentSlot?.Invoke(CurrentSlot);
    }
}
