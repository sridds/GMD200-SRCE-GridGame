using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Playing,
    Paused,
    UI
}

public class GameManager : MonoBehaviour
{
    [Header("Global Refrences")]

    public static GameManager Instance;

    public Transform player;

    public ItemGrid inventory;

    [Header("Current Day")]
    public int day;
    public Transform startPos;

    public float currentDayTimer;
    public float maxDayTimer = 20;

    public GameState currentGameState;
    void Awake()
    {
        //Destroy if instance already exsists
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    private void Update()
    {
        currentDayTimer += Time.deltaTime;

        if (currentDayTimer >= maxDayTimer)
        {
            GameManager.Instance.NextDay();
            player.transform.position = startPos.transform.position;
        }
    }
    /// <summary>
    /// Changes the current state of the game, will call UImanagers etc.
    /// </summary>
    /// <param name="state"></param>
    public void UpdateGameState(GameState state)
    {
        switch (state)
        {
            case GameState.Playing:
//                UIManager.Instance.Pause(false);
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1f;
                break;

            case GameState.Paused:
                UIManager.Instance.Pause(true);
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0f;
                break;
            case GameState.UI:
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 1f;
                break;
        }
    }
    [ContextMenu("Button")]
    public void NextDay()
    {
        day++;
        currentDayTimer = 0;
        UIManager.Instance.DayTransitionUI();
    }
}
