using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class Health : MonoBehaviour, IStatHandler
{
    public enum HealthMode
    {
        Value,
        Increments
    }

    [Header("Health")]
    [SerializeField]
    private HealthMode mode;

    [field: SerializeField]
    public Stat myStat { get; private set; }

    [Header("IFrames")]
    [SerializeField]
    private bool _doIFrames = true;

    [ShowIf(nameof(_doIFrames))]
    [SerializeField]
    private int _maxIFrames = 30;
    [ShowIf(nameof(_doIFrames))]
    [SerializeField]
    private float _IFrameInterval = 0.05f;
    [ShowIf(nameof(_doIFrames))]
    [SerializeField]
    private SpriteRenderer _blinker;

    [Header("Hitmarker")]
    [SerializeField]
    private bool _doHitmarker = true;

    // ACCESSORS
    public int CurrentHealth { get => (int)myStat.CurrentValue; }
    public int MaxHealth { get => (int)myStat.MaxValue; }
    public bool IFramesActive { get; private set; }

    private bool canDamage = true;
    private bool healthDepleted = false;

    // EVENTS
    public delegate void HealthIncrease(int newHealth);
    public HealthIncrease OnHealthIncrease;

    public delegate void HealthDecrease(int newHealth);
    public HealthIncrease OnHealthDecrease;

    public delegate void HealthDepleted();
    public HealthDepleted OnHealthDepleted;

    // set health
    private void Start() => myStat.Init();

    /// <summary>
    /// Takes damage and calls an event
    /// </summary>
    /// <param name="damageAmount"></param>
    public void DecreaseStat(float damageAmount)
    {
        if (!canDamage) return;
        if (healthDepleted) return;

        int value = (int)(mode == HealthMode.Value ? damageAmount : 1);
        // Decrease stat
        myStat.Decrease(value);

        // call the iframes coroutine
        if (_doIFrames) StartCoroutine(HandleIFrames(_maxIFrames, _IFrameInterval));

        // Create hitmarker
        if (_doHitmarker) {
            Vector2 pos = new Vector2(Random.Range(transform.position.x - 0.4f, transform.position.x + 0.4f), Random.Range(transform.position.y - 0.4f, transform.position.y + 0.4f));
            Hitmarker.CreateHitmarker(pos, value);
        }

        // call events
        if (myStat.CurrentValue <= 0) {
            OnHealthDepleted?.Invoke();
            healthDepleted = true;
        }
        else {
            OnHealthDecrease?.Invoke(CurrentHealth);
        }
    }

    /// <summary>
    /// An external call to start IFrames. Stops the current IFrame coroutine.
    /// </summary>
    /// <param name="iframes"></param>
    /// <param name="interval"></param>
    public void CallIFrames(int iframes, float interval)
    {
        StopAllCoroutines();

        StartCoroutine(HandleIFrames(iframes, interval));
    }

    /// <summary>
    /// Handles the IFrames (another amazing summary by yours truly)
    /// </summary>
    /// <returns></returns>
    private IEnumerator HandleIFrames(int iframes, float interval)
    {
        IFramesActive = true;
        canDamage = false;
        // blink the sprite renderer for the set number of iframes
        for (int i = 0; i < iframes; i++)
        {
            yield return new WaitForSeconds(interval);
            _blinker.enabled = false;
            yield return new WaitForSeconds(interval);
            _blinker.enabled = true;
        }
        // just in case its still disabled
        _blinker.enabled = true;

        IFramesActive = false;
        canDamage = true;
    }

    public void IncreaseStat(float amount)
    {
        myStat.Increase(amount);

        OnHealthIncrease?.Invoke(CurrentHealth);
    }
}