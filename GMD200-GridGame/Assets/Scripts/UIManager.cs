using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    private Transition transitionManager;

    [Header("Pause Settings")]
    [SerializeField] private GameObject pauseMenu;

    [Header("Day Transition Refrences")]

    [SerializeField] private Image dayTransitionBG;

    [SerializeField] private TextMeshProUGUI dayTransitionText;

    [Tooltip("The target alpha")]
    [Range(0, 1)]
    [SerializeField] private float targetTransitionAlpha = 1f;

    [Tooltip("The time it takes to fade into black")]
    [SerializeField] private float transitionTime = 1f;

    [Tooltip("The time until it fades out of black")]
    [SerializeField] private float fadeOutWaitTime = 1f;


    [SerializeField] private TextMeshProUGUI dayText;

    public static UIManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        transitionManager = GetComponentInChildren<Transition>();
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
        //Transition text and background
        transitionManager.StartTransition(dayTransitionBG, targetTransitionAlpha, transitionTime, fadeOutWaitTime);
        transitionManager.StartTransition(dayTransitionText, targetTransitionAlpha, transitionTime, fadeOutWaitTime);

        dayText.text = $"Day {GameManager.Instance.day}";
    }
}
