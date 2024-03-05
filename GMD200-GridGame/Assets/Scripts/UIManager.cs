using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    [Header("Timer Settings")]

    public float timer = 300;

    [SerializeField] private TextMeshProUGUI timeText;

    [Header("Pause Settings")]
    [SerializeField] private GameObject pauseMenu;

    [Header("Resource Settings")]
    [SerializeField] private TextMeshProUGUI resourcesText;

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
        timer -= Time.deltaTime;
        float minutes = Mathf.FloorToInt(timer / 60);
        float seconds = Mathf.FloorToInt(timer % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void Pause(bool isPaused)
    {
        if (!isPaused)
            pauseMenu.SetActive(false);
        else
            pauseMenu.SetActive(true);
    }
    public void UpdateResources(int wood, int stone)
    {
        resourcesText.text = $"Wood: {wood} \n Stone: {stone}";
    }
}
