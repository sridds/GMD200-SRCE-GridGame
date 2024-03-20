using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private NewMovement movement;

    [SerializeField]
    private SpriteRenderer renderer;
    [SerializeField]
    private float spriteChangeTime = 0.25f;

    [SerializeField]
    private Sprite[] leftSprites;
    [SerializeField]
    private Sprite[] rightSprites;
    [SerializeField]
    private Sprite[] upSprites;
    [SerializeField]
    private Sprite[] downSprites;

    private float timer;
    int index = 0;
    Sprite[] activeSprites;

    void Start()
    {
        movement = GetComponent<NewMovement>();
        activeSprites = downSprites;
    }

    void Update()
    {
        Sprite[] arr = activeSprites;

        if (movement.IsMoving)
        {
            if (movement.MyDirection == NewMovement.Direction.Left) arr = leftSprites;
            else if (movement.MyDirection == NewMovement.Direction.Right) arr = rightSprites;
            else if (movement.MyDirection == NewMovement.Direction.Down) arr = downSprites;
            else if (movement.MyDirection == NewMovement.Direction.Up) arr = upSprites;

            timer += Time.deltaTime;

            if (arr != activeSprites) {
                activeSprites = arr;
                renderer.sprite = activeSprites[index];
            }

            if (timer > spriteChangeTime) {
                renderer.sprite = activeSprites[index];
                index++;
                index = index % activeSprites.Length;

                timer = 0.0f;
            }
        }
        else
        {
            timer = 0.0f;
            index = 0;
            renderer.sprite = activeSprites[index];
        }
    }
}
