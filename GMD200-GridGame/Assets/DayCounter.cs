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
        UpdateCount();
    }

    void UpdateCount() => text.SetText("DAY " + GameManager.Instance.day);
}
