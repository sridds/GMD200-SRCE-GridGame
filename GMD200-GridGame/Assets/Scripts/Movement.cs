using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(ResourceCollection))]
public class Movement : MonoBehaviour
{
    private ResourceCollection collect;

    private PerlinData data;

    [SerializeField] private float moveSpeed = 3f;

    private void Start()
    {
        collect = GetComponent<ResourceCollection>();

        data = PerlinData.Instance;
    }

    private void Update()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        if (xInput == 0 && yInput == 0) return;

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
            isMoving = StartCoroutine(MoveToPoint(newPosX, newPosY, moveSpeed));
    }
    Coroutine isMoving = null;
    IEnumerator MoveToPoint(int x, int y, float duration)
    {
        Vector2 startPos = transform.position;
        Vector2 targetPos = data.tiles[x, y].tilePosition;
        float time = 0;
        while (time <= duration)
        {
            time += Time.deltaTime;
            transform.position = Vector2.Lerp(startPos, targetPos, time / duration);
            yield return null;
        }
        transform.position = targetPos;
        isMoving = null;
    }
}
