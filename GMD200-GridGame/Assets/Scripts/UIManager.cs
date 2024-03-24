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
    public void Pause(bool isPaused)
    {
        if (!isPaused)
            pauseMenu.SetActive(false);
        else
            pauseMenu.SetActive(true);
    }
    /// <summary>
    /// Displays the day transition
    /// </summary>
    public void DayTransitionUI()
    {
        //Transition text and background
        transitionManager.StartTransition(dayTransitionBG);
        transitionManager.StartTransition(dayTransitionText);

        dayText.text = $"DAY {GameManager.Instance.day}";
    }
}
