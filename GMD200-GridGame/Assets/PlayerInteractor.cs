using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    NewMovement movement;

    [Header("Ray Settings")]
    [SerializeField]
    private float rayDistance = 2.0f;
    [SerializeField]
    private Vector2 rayOffset = new Vector2(0, 0.5f);
    [SerializeField]
    private float interactionCooldown = 0.3f;
    [SerializeField]
    private LayerMask interactableLayer;

    float interactionCooldownTimer = 0.0f;
    public int CurrentSlot { get; private set; }

    public delegate void CurrentSlotUpdate(int slot);
    public CurrentSlotUpdate OnUpdateCurrentSlot;

    private void Awake() => movement = GetComponent<NewMovement>();

    void Update()
    {
        // increment timers
        interactionCooldownTimer += Time.deltaTime;

        UpdateCurrentSlot(); // update current slot input

        // handle interaciton
        if (CanInteract()) {
            Interact();
        }
    }

    /// <summary>
    /// Gets scroll input and updates the current slot index accordingly
    /// </summary>
    private void UpdateCurrentSlot()
    {
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

    /// <summary>
    /// Handles the interaction
    /// </summary>
    private void Interact()
    {
        interactionCooldownTimer = 0.0f;
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y) + rayOffset, movement.GetDirectionFacing(), rayDistance, interactableLayer);
        if (hit.collider == null) return;


        // check for interactable
        if (hit.collider.TryGetComponent<Interactable>(out Interactable interactable))
        {
            interactable.Interact();
        }

        // check for health
        else if (hit.collider.TryGetComponent<Health>(out Health health))
        {
            health.TakeDamage(1);
        }
    }

    private bool CanInteract()
    {
        if (interactionCooldownTimer < interactionCooldown) return false;
        if (!Input.GetKeyDown(KeyCode.E)) return false;

        return true;
    }
}
