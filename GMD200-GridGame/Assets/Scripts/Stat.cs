using System;
using UnityEngine;

[Serializable]
public class Stat
{
    public event Action OnValueZero;
    public event Action OnValueChanged;

    // serializable in the inspector and also accessible to others
    [field: SerializeField] public float MaxValue { get; private set; }

    private float currentValue;

    public float CurrentValue
    {
        // returns the current value (private)
        get => currentValue;
        set
        {
            currentValue = Mathf.Clamp(value, 0f, MaxValue);

            if (currentValue <= 0f)
            {
                OnValueZero?.Invoke();
            }
        }
    }

    public void Init()
    {
        CurrentValue = MaxValue;
        OnValueChanged?.Invoke();
    }

    public void Increase(float amount)
    {
        CurrentValue += amount;
        OnValueChanged?.Invoke();
    }
    public void Decrease(float amount)
    {
        CurrentValue -= amount;
        OnValueChanged?.Invoke();
    }
}