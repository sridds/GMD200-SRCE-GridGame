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

    public float collectCooldown = 0.5f;

    public int damage = 1;

    private bool isPaused = false;

    private bool canCollect = true;

    private void Start()
    {
        collect = GetComponent<ResourceCollection>();

        data = PerlinData.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Pause();

            Move();

        //Debug
        if (Input.GetKeyDown(KeyCode.R))
            PerlinData.Instance.GenerateNewGrid(85, 85);
    }
    void Pause()
    {
        isPaused = !isPaused;

        if (isPaused)
            GameManager.Instance.UpdateGameState(GameState.paused);
        else
            GameManager.Instance.UpdateGameState(GameState.playing);
    }

    void Move()
    {
        //Get Inputs
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        //If there are no inputs, don't run code
        if (xInput == 0 && yInput == 0) return;

        //Static cast from float to int
        int newPosX = (int)(transform.position.x + xInput);
        int newPosY = (int)(transform.position.y + yInput);

        //Impassible Terrain Check
        if (!PerlinData.Instance.InBounds(newPosX, newPosY)) return;

        if (data.tiles[newPosX, newPosY].tileType == TileType.Water) return;

        int lastPosX = (int)transform.position.x;
        int lastPosY = (int)transform.position.y;

        //Resource Collection
        if (data.tiles[newPosX, newPosY].resource != null)
        {
            //Stop current coroutine
            StopCoroutine(MoveToPoint(newPosX, newPosY, speed));

            if (isMoving == null)
                isMoving = StartCoroutine(MoveToPoint(lastPosX, lastPosY, speed));

            if (canCollect)
            {
                canCollect = false;
                collect.CollectResource(newPosX, newPosY, data.tiles[newPosX, newPosY], damage);
                Invoke(nameof(ResetCollectCooldown), 0.5f);
            }
        }

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

    private void ResetCollectCooldown() => canCollect = true;
}
