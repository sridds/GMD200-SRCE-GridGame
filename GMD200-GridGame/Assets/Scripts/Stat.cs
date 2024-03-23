using System;
using UnityEngine;

[Serializable]
public struct Stat : IStat
{
    public event Action OnValueZero;

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

    public void Init() => CurrentValue = MaxValue;

    public void Increase(float amount) => CurrentValue += amount;
    public void Decrease(float amount) => CurrentValue -= amount;
}

public interface IStat
{
    public float CurrentValue { get; }
    public float MaxValue { get; }

    void Increase(float amount);
    void Decrease(float amount);
}