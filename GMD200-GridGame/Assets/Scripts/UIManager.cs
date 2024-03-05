using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    [Header("Timer Settings")]

    public float timer = 360;

    [SerializeField] private TextMeshProUGUI timeText;

    private void Update()
    {
        float minutes = Mathf.FloorToInt(timer / 60);
        float seconds = Mathf.FloorToInt(timer % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
