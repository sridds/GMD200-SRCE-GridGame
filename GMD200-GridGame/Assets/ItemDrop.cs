using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    private Rigidbody2D rb;

    [Tooltip("Indicates how long it takes until the item can be picked up by the player")]
    [SerializeField] private float _spawnCooldown = 1.0f;

    [Header("Magnet Settings")]
    [SerializeField] private float _magnetSpeed = 5.0f;
    [SerializeField] private float _magnetRadius = 3.0f;

    [Header("References")]
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private SpriteRenderer _multiSpriteIndicator;

    private int stack;
    private ItemSO myItem;
    private Transform target;

    private Timer cooldownTimer;

    /// <summary>
    /// Initalizes the item drop
    /// </summary>
    /// <param name="target"></param>
    /// <param name="myItem"></param>
    /// <param name="stack"></param>
    public void Init(Transform target, ItemSO myItem, int stack = 1)
    {
        this.myItem = myItem;
        this.stack = stack;
        this.target = target;

        if (myItem != null) _renderer.sprite = myItem.ItemSprite;
        if (stack > 1) {
            _multiSpriteIndicator.enabled = true;
            _multiSpriteIndicator.sprite = _renderer.sprite;
        }

        // create timer and set timer to null once it ends
        cooldownTimer = new Timer(_spawnCooldown);
        cooldownTimer.OnTimerEnd += () => cooldownTimer = null;

        rb = GetComponent<Rigidbody2D>();

        // add initial force
        rb.AddForce(Random.insideUnitCircle * 2.0f, ForceMode2D.Impulse);
    }

    private void Update()
    {
        // tick timer
        if (cooldownTimer != null) cooldownTimer.Tick(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        // guard clauses
        if (cooldownTimer != null) return; // is the timer still ticking?
        if (target == null) return;
        if (GameManager.Instance.inventory.IsFullOfItem(myItem)) return;

        // magnet
        if (Vector2.Distance(transform.position, target.position) < _magnetRadius)
        {
            Vector2 targetDirection = (target.position - transform.position).normalized;
            rb.AddForce(targetDirection * _magnetSpeed, ForceMode2D.Force);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (cooldownTimer != null) return; // is the timer still ticking?
        if (target == null) return;
        if (collision.gameObject.tag != "Player") return;

        int stackCount = stack;
        int num = 0;

        for (int i = 0; i < stackCount; i++)
        {
            if (!GameManager.Instance.inventory.AddItem(myItem)) return;
            stack--;
            num++;

            if (stack == 1) _multiSpriteIndicator.enabled = false;
        }

        Destroy(gameObject);
    }
}
