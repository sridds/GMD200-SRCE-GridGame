using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DayCounter : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    private void Start()
    {
        GameManager.Instance.OnDayUpdate += UpdateCount;
        GameManager.Instance.OnDayUpdate += CheckDay;
        UpdateCount();
    }

    void CheckDay()
    {
        if (GameManager.Instance.day >= 10)
        {
            GameManager.Instance.Death();
        }
    }

    void UpdateCount() => text.SetText("DAY " + GameManager.Instance.day);
}
