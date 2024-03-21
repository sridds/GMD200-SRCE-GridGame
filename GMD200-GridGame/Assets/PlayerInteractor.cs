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

    float interactionCooldownTimer = 0.0f;

    private void Awake() => movement = GetComponent<NewMovement>();

    void Update()
    {
        interactionCooldownTimer += Time.deltaTime;

        if (interactionCooldownTimer < interactionCooldown) return;

        if (Input.GetKeyDown(KeyCode.E)) {

            interactionCooldownTimer = 0.0f;
            // draw debug ray
            Debug.DrawRay(new Vector2(transform.position.x, transform.position.y) + rayOffset, movement.GetDirectionFacing() * rayDistance, Color.magenta, 2.0f);
        }
    }
}
