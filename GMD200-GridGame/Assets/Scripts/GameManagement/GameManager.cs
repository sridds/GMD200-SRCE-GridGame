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

        //Subscribe death to OnHealthDepleted event 
        player.GetComponent<Health>().OnHealthDepleted += Death;
    }

    private void Update()
    {
        currentDayTimer += Time.deltaTime;
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
    /// <summary>
    /// Transitions to the next day
    /// </summary>
    public void NextDay()
    {
        day++;
        currentDayTimer = 0;
        UIManager.Instance.DayTransitionUI();
    }

    private void Death()
    {
        GetComponent<PostProcessManager>().DeathVisuals();

    }
}
