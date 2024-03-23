using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    NewMovement movement;
    InventoryController controller;

    [Header("Ray Settings")]
    [SerializeField]
    private float rayDistance = 2.0f;
    [SerializeField]
    private Vector2 rayOffset = new Vector2(0, 0.5f);
    [SerializeField]
    private float interactionCooldown = 0.3f;
    [SerializeField]
    private LayerMask interactableLayer;

    [SerializeField]
    private ToolSO tool;
    [SerializeField]
    private WeaponSO weapon;

    InteractionText text;

    float interactionCooldownTimer = 0.0f;

    private void Start() {
        movement = GetComponent<NewMovement>();
        controller = FindObjectOfType<InventoryController>();
        text = FindObjectOfType<InteractionText>();
    }

    void Update()
    {
        if (GameManager.Instance.currentGameState == GameState.Paused || GameManager.Instance.currentGameState == GameState.UI) return;

        ShowInteractionText();

        //DEBUG
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F))
        {
            GameManager.Instance.inventory.AddItem(tool);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            GameManager.Instance.inventory.AddItem(weapon);
        }
#endif
        // increment timers
        interactionCooldownTimer += Time.deltaTime;

        if (CanUseItem()) UseItem();
        if (CanInteract()) Interact();
    }

    /// <summary>
    /// Handles the usage of an item
    /// </summary>
    private void UseItem()
    {
        interactionCooldownTimer = 0.0f;

        TryCastInteractionRay(out RaycastHit2D hit);
        ItemSO item = GameManager.Instance.inventory.GetSlot(controller.CurrentSlot, 0).Item;
        if (item == null) return;

        // use
        item.OnUse(new UseContext(hit, GameManager.Instance.inventory.GetSlot(controller.CurrentSlot, 0)));
    }

    private void ShowInteractionText()
    {
        if (!TryCastInteractionRay(out RaycastHit2D hit) || !hit.collider.TryGetComponent<Interactable>(out Interactable interactable))
        {
            text.HideInteractionText();
            return;
        }

        text.SetInteractionText(interactable.InteractText);
    }

    /// <summary>
    /// Handle Interaction
    /// </summary>
    private void Interact()
    {
        interactionCooldownTimer = 0.0f;

        if (!TryCastInteractionRay(out RaycastHit2D hit)) return;

        // check for basic interactions
        if (hit.collider.TryGetComponent<Interactable>(out Interactable interactable))
        {
            interactable.Interact();
        }
    }

    /// <summary>
    /// Casts a ray in the direction of the player. Returns true if hit, outputting RaycastHit2D data
    /// </summary>
    /// <param name="hit"></param>
    /// <returns></returns>
    private bool TryCastInteractionRay(out RaycastHit2D hit)
    {
        hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y) + rayOffset, movement.GetDirectionFacing(), rayDistance, interactableLayer);
        if (hit.collider == null) return false;

        return true;
    }

    /// <summary>
    /// Returns true if the player can use an item. Otherwise, return false. Just a wrapped boolean for readability
    /// </summary>
    /// <returns></returns>
    private bool CanUseItem()
    {
        if (interactionCooldownTimer < interactionCooldown) return false;
        if (!Input.GetMouseButton(0)) return false;

        return true;
    }

    private bool CanInteract() {
        if (interactionCooldownTimer < interactionCooldown) return false;
        if (!Input.GetMouseButtonDown(1)) return false;

        return true;
    }
}
