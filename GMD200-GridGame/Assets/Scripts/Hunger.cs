using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatHandler
{
    public Stat myStat { get; }

    void IncreaseStat(float amount);
    void DecreaseStat(float amount);
}

public class Hunger : MonoBehaviour, IStatHandler
{
    [field: SerializeField]
    public Stat myStat { get; private set; }

    [field: SerializeField]
    public Stat saturation { get; private set; }

    [SerializeField]
    private float hungerDecreaseRate;

    [SerializeField]
    private float saturationDecreaseRate;

    [SerializeField]
    private float healthDecreaseRate;

    [SerializeField]
    private float saturationIgnoreThreshold;

    [SerializeField]
    private AnimationCurve hungerRateCurve;

    [SerializeField]
    private Health playerHealth;

    private float saturationDecreaseTimer;
    private float hungerDecreaseTimer;
    private float healthDecreaseTimer;

    private void Start()
    {
        myStat.Init();
        saturation.Init();
    }

    /// <summary>
    /// Takes in consumable data and processes it, increasing the hunger stat.
    /// </summary>
    /// <param name="consumable"></param>
    public void IncreaseStat(ConsumableSO consumable)
    {
        myStat.Increase(consumable.HealAmount);
        saturation.Increase(consumable.SaturationAmount);
    }

    /// <summary>
    /// provides a small boost in hunger
    /// </summary>
    /// <param name="amount"></param>
    public void IncreaseStat(float amount) => myStat.Increase(amount);

    public void DecreaseStat(float amount) => myStat.Decrease(amount);

    public void DecreaseSaturation(float amount) => saturation.Decrease(amount);

    public void IncreaseSaturation(float amount) => saturation.Increase(amount);

    private void Update()
    {
        if (GameManager.Instance.currentGameState == GameState.Paused) return;

        UpdateSaturation();
        UpdateHunger();

        // decrease health
        if(myStat.CurrentValue == 0) {
            healthDecreaseTimer += Time.deltaTime;

            if(healthDecreaseTimer > healthDecreaseRate) {
                // decrease health
                playerHealth.DecreaseStat(1);

                healthDecreaseTimer = 0.0f;
            }
        }
        else {
            healthDecreaseTimer = 0.0f;
        }
    }

    private void UpdateSaturation()
    {
        if (saturation.CurrentValue <= 0) return;

        saturationDecreaseTimer += Time.deltaTime;

        // this is the standard way of decreasing saturation
        if (saturationDecreaseTimer > saturationDecreaseRate)
        {
            saturationDecreaseTimer = 0.0f;
            saturation.Decrease(1);
        }
    }

    private void UpdateHunger()
    {
        if (saturation.CurrentValue > saturationIgnoreThreshold) return;

        hungerDecreaseTimer += Time.deltaTime;

        if (hungerDecreaseTimer > (hungerDecreaseRate * hungerRateCurve.Evaluate(saturation.CurrentValue / saturation.MaxValue)))
        {
            hungerDecreaseTimer = 0.0f;
            myStat.Decrease(1);
        }
    }
}
