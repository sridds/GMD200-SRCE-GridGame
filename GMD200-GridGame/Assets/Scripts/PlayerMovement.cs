using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    public Transform movePoint;

    public LayerMask stopsMovement;

    private Vector2 currentPosition;

    private void Start()
    {
        movePoint.parent = null;
    }

    private void Update()
    {

        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        int newPosX = Mathf.FloorToInt(transform.position.x + xInput);
        int newPosY = Mathf.FloorToInt(transform.position.y + yInput);

        /*Debug.Log(PerlinData.Instance.tiles[newPosX, newPosY].tileType);
        Debug.Log(newPosX);
        Debug.Log(newPosY);*/

        if (!PerlinData.Instance.InBounds(newPosX, newPosY))
            return;
        if (PerlinData.Instance.tiles[newPosX, newPosY].tileType == TileType.Water)
            return;

        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed* Time.deltaTime);

        if(Vector3.Distance(transform.position, movePoint.position) <= 0.05)
        {
            if (Mathf.Abs(xInput) == 1f)
            {
                //movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                movePoint.position = PerlinData.Instance.tiles[newPosX, newPosY].tilePosition;
            }
            else if (Mathf.Abs(yInput) == 1f)
            {
                //movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                movePoint.position = PerlinData.Instance.tiles[newPosX, newPosY].tilePosition;
            }
            
        }
    }
}
