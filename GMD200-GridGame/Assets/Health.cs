using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class Health : MonoBehaviour
{
    public enum HealthMode
    {
        Value,
        Increments
    }

    [Header("Health")]
    [SerializeField]
    private HealthMode mode;

    [ShowIf(nameof(mode), HealthMode.Value)]
    [SerializeField, Min(1)]
    private int _maxHealth;

    [ShowIf(nameof(mode), HealthMode.Increments)]
    [SerializeField, Min(1)]
    private int _maxHits;

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

    // ACCESSORS
    public int CurrentHealth { get; private set; }
    public int MaxHealth { get { return mode == HealthMode.Value ? _maxHealth : _maxHits; } }
    public bool IFramesActive { get; private set; }

    private bool canDamage = true;
    private bool healthDepleted = false;

    // EVENTS
    public delegate void HealthUpdate(int oldHealth, int newHealth);
    public HealthUpdate OnHealthUpdate;

    public delegate void HealthDepleted();
    public HealthDepleted OnHealthDepleted;

    // set health
    private void Start() => CurrentHealth = mode == HealthMode.Value ? _maxHealth : _maxHits;

    /// <summary>
    /// Takes damage and calls an event
    /// </summary>
    /// <param name="damageAmount"></param>
    public void TakeDamage(int damageAmount)
    {
        if (!canDamage) return;
        if (healthDepleted) return;

        CurrentHealth -= mode == HealthMode.Value ? damageAmount : 1;

        // call the iframes coroutine
        if (_doIFrames) StartCoroutine(HandleIFrames(_maxIFrames, _IFrameInterval));

        // call events
        if (CurrentHealth <= 0)
        {
            OnHealthDepleted?.Invoke();
            healthDepleted = true;
        }
        else
        {
            OnHealthUpdate?.Invoke(CurrentHealth + damageAmount, CurrentHealth);
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
}
