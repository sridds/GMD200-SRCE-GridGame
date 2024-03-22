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

    float interactionCooldownTimer = 0.0f;

    private void Start() {
        movement = GetComponent<NewMovement>();
        controller = FindObjectOfType<InventoryController>();
    }

    void Update()
    {
        if (GameManager.Instance.currentGameState == GameState.Paused || GameManager.Instance.currentGameState == GameState.UI) return;

        //DEBUG
        if (Input.GetKeyDown(KeyCode.F))
        {
            GameManager.Instance.inventory.AddItem(tool);
        }

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
        if (!TryCastInteractionRay(out RaycastHit2D hit)) return;

        // check for health
        //if (hit.collider.TryGetComponent<Health>(out Health health)) health.TakeDamage(1);
        if(hit.collider.TryGetComponent<IBreakable>(out IBreakable breakable)) {

            ItemSO item = GameManager.Instance.inventory.GetSlot(controller.CurrentSlot, 0).Item;
            // i dont like this
            if (item is ToolSO) breakable.Damage(item as ToolSO);
        }
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
        if (!Input.GetMouseButtonDown(0)) return false;

        return true;
    }

    private bool CanInteract() {
        if (interactionCooldownTimer < interactionCooldown) return false;
        if (!Input.GetMouseButtonDown(1)) return false;

        return true;
    }
}
