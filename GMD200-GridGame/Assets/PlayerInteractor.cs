using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    NewMovement movement;

    [SerializeField]
    private float rayDistance = 2.0f;
    [SerializeField]
    private Vector2 rayOffset = new Vector2(0, 0.5f);
    [SerializeField]
    private float interactionCooldown = 0.3f;
    [SerializeField]
    private LayerMask interactableLayer;

    float interactionCooldownTimer = 0.0f;

    private void Awake() => movement = GetComponent<NewMovement>();

    void Update()
    {
        interactionCooldownTimer += Time.deltaTime;

        if (interactionCooldownTimer < interactionCooldown) return;

        if (Input.GetKeyDown(KeyCode.E)) {

            interactionCooldownTimer = 0.0f;
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y) + rayOffset, movement.GetDirectionFacing(), rayDistance, interactableLayer);
            if (hit.collider == null) return;


            // check for interactable
            if (hit.collider.TryGetComponent<Interactable>(out Interactable interactable))
            {
                interactable.Interact();
            }

            // check for health
            else if(hit.collider.TryGetComponent<Health>(out Health health))
            {
                health.TakeDamage(1);
            }
        }
    }
}
