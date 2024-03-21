using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMovement : MonoBehaviour
{
    public enum Direction { Left, Right, Down, Up }

    [SerializeField] private float _moveSpeed = 4.0f;

    private Vector2 input;
    private Rigidbody2D rb;

    // getters
    public Direction MyDirection;
    public bool IsMoving { get { return rb.velocity.magnitude > 0.01f; } }

    private void Awake() => rb = GetComponent<Rigidbody2D>();

    private void Update()
    {
        GetInputs();

        if (input.x > 0) MyDirection = Direction.Right;
        else if (input.x < 0) MyDirection = Direction.Left;

        if (input.y > 0) MyDirection = Direction.Up;
        else if (input.y < 0) MyDirection = Direction.Down;
    }

    private void GetInputs()
    {
        // get inputs
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        input.Normalize();
    }

    public Vector2 GetDirectionFacing()
    {
        Vector2 dir = Vector2.zero;

        switch (MyDirection)
        {
            case Direction.Right:
                dir = Vector2.right;
                break;
            case Direction.Left:
                dir = Vector2.left;
                break;
            case Direction.Up:
                dir = Vector2.up;
                break;
            case Direction.Down:
                dir = Vector2.down;
                break;
        }
        return dir;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(input.x, input.y) * _moveSpeed;
    }
}
