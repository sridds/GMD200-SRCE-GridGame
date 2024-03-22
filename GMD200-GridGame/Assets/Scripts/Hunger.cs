using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunger : MonoBehaviour
{
    [SerializeField]
    private Stat hungerStat;

    [SerializeField]
    private StatDecreaser hungerDecreaseModifier;


    private void Start()
    {
        hungerDecreaseModifier = new StatDecreaser(hungerStat);
    }

    /// <summary>
    /// Takes in consumable data and processes it, increasing the hunger stat.
    /// </summary>
    /// <param name="consumable"></param>
    public void Consume(ConsumableSO consumable)
    {
        hungerStat.Increase(consumable.HealAmount);
    }

    private void Update()
    {
        hungerDecreaseModifier.Tick();
    }
}

/// <summary>
/// Decreases a stat over time
/// </summary>
public class StatDecreaser
{
    Stat stat;

    public StatDecreaser(Stat stat) => this.stat = stat;

    public void Tick()
    {
    }
}