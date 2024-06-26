using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField]
    private RectTransform inventoryRect;

    [SerializeField]
    private float _slotSwitchCooldown = 0.05f;

    [SerializeField]
    private string bagSoundKey = "bag";

    public int CurrentSlot { get; private set; }
    private bool inventoryDisplaying;

    public delegate void CurrentSlotUpdate(int slot);
    public CurrentSlotUpdate OnUpdateCurrentSlot;

    float slotSwitchTimer;

    private void Update()
    {
        if (GameManager.Instance.currentGameState == GameState.Paused) return;

        UpdateCurrentSlot(); // update current slot input
        if (Input.GetKeyDown(KeyCode.Q) && !inventoryDisplaying) DropCurrentItem();

        // toggles the inventory on and off
        if (Input.GetKeyDown(KeyCode.E)) ToggleInventory();
    }

    private void DropCurrentItem()
    {
        GameManager.Instance.inventory.DropFromSlot(CurrentSlot, 0);
    }

    public void ToggleInventory()
    {
        if (!inventoryDisplaying && GameManager.Instance.currentGameState == GameState.UI) return;

        inventoryDisplaying = !inventoryDisplaying;
        inventoryRect.gameObject.SetActive(inventoryDisplaying);

        if (inventoryDisplaying)
        {
            AudioHandler.instance.ProcessAudioData(transform, bagSoundKey);
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
        if (slotSwitchTimer > 0.0f) {
            slotSwitchTimer -= Time.deltaTime;
            return;
        }

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
        slotSwitchTimer = _slotSwitchCooldown;

        OnUpdateCurrentSlot?.Invoke(CurrentSlot);
    }
}
