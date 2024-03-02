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
    public Transform playerPos;

    public static GameManager Instance;

    public GameState currentGameState;
    void Start()
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
                //Call UI manager and Cursor manager
                Time.timeScale = 1f;
                break;

            case GameState.paused:
                //Call UI manager and Cursor manager
                Time.timeScale = 0f;
                break;
        }
    }
}
