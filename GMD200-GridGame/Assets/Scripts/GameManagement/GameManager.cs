using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameState
{
    playing,
    paused,
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform player;

    [Header("Resources")]
    [SerializeField] private int wood;
    [SerializeField] private int stone;

    [Header("Current Day")]
    public int day;

    public GameState currentGameState;
    void Awake()
    {
        //Destroy if instance already exsists
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
    /// <summary>
    /// Changes the current state of the game, will call UImanagers etc.
    /// </summary>
    /// <param name="state"></param>
    public void UpdateGameState(GameState state)
    {
        switch (state)
        {
            case GameState.playing:
                UIManager.Instance.Pause(false);
                CursorManager.setCursorMode?.Invoke(CursorLockMode.Locked);
                Time.timeScale = 1f;
                break;

            case GameState.paused:
                UIManager.Instance.Pause(true);
                CursorManager.setCursorMode?.Invoke(CursorLockMode.None);
                Time.timeScale = 0f;
                break;
        }
    }

    public void AddResource(TileType resourceType, int amount)
    {
        switch (resourceType)
        {
            case TileType.Tree:
                wood += amount;
                break;

            case TileType.Rock:
                stone += amount;
                break;
        }
        UIManager.Instance.UpdateResources(wood, stone);
    }
}
