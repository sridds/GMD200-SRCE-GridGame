using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatUI : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private string statName;

    [SerializeField]
    private UnityEngine.Object _statHandler; // works
 
    public IStatHandler statHandler => _statHandler as IStatHandler;

    /// <summary>
    /// Initialize values
    /// </summary>
    void Start()
    {
        statHandler.myStat.OnValueChanged += UpdateValue;

        UpdateValue();
    }

    /// <summary>
    /// Called by an event inside the stat
    /// </summary>
    private void UpdateValue()
    {
        text.SetText($"{statHandler.myStat.CurrentValue} {statName}");

        slider.maxValue = statHandler.myStat.MaxValue;
        slider.value = statHandler.myStat.CurrentValue;
    }
}
