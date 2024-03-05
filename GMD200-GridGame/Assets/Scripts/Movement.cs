using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(ResourceCollection))]
public class Movement : MonoBehaviour
{
    private ResourceCollection collect;

    private PerlinData data;

    [Header("Player Settings")]

    [SerializeField] private float speed = 3f;

    private void Start()
    {
        collect = GetComponent<ResourceCollection>();

        data = PerlinData.Instance;
    }

    private void Update()
    {
        //Get Inputs
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        //If there are no inputs, don't run code
        if (xInput == 0 && yInput == 0) return;

        //Round the new position into an int
        int newPosX = Mathf.FloorToInt(transform.position.x + xInput);
        int newPosY = Mathf.FloorToInt(transform.position.y + yInput);

        //Impassible Terrain Check
        if (!PerlinData.Instance.InBounds(newPosX, newPosY)) return;

        if (data.tiles[newPosX, newPosY].tileType == TileType.Water) return;

        //Resource Collection
        if (data.tiles[newPosX, newPosY].tileType == TileType.Tree)
            collect.CollectResource(newPosX, newPosY, TileType.Tree);

        if (data.tiles[newPosX, newPosY].tileType == TileType.Rock)
            collect.CollectResource(newPosX, newPosY, TileType.Rock);

        //Start movement
        if (isMoving == null)
            isMoving = StartCoroutine(MoveToPoint(newPosX, newPosY, speed));
    }
    Coroutine isMoving = null;
    IEnumerator MoveToPoint(int x, int y, float duration)
    {
        //Initialize
        Vector2 startPos = transform.position;
        Vector2 targetPos = data.tiles[x, y].tilePosition;
        float time = 0;

        //Move player to position
        while (time <= duration)
        {
            time += Time.deltaTime;
            transform.position = Vector2.Lerp(startPos, targetPos, time / duration);
            yield return null;
        }

        //Set player to be at position
        transform.position = targetPos;
        isMoving = null;
    }
}
