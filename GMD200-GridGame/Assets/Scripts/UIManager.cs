using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    [Header("Pause Settings")]
    [SerializeField] private GameObject pauseMenu;

    [Header("Day Transition Refrences")]

    [SerializeField] private GameObject dayTransition;

    [SerializeField] private TextMeshProUGUI dayText;

    public static UIManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
    private void Update()
    {
        /*timer -= Time.deltaTime;
        float minutes = Mathf.FloorToInt(timer / 60);
        float seconds = Mathf.FloorToInt(timer % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);*/

        /*//Resize and regenerate map
        if (timer <= 0)
        {
            PerlinData.Instance.GenerateNewGrid(85, 85);
            timer = 2;
        }*/
    }

    public void Pause(bool isPaused)
    {
        if (!isPaused)
            pauseMenu.SetActive(false);
        else
            pauseMenu.SetActive(true);
    }
    public void DayTransitionUI()
    {
        dayTransition.SetActive(true);
        dayText.text = GameManager.Instance.day.ToString();
    }
    public void DisableTransition()
    {
        dayTransition.SetActive(false);
    }
}
