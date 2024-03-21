using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    private Rigidbody2D rb;

    [Tooltip("Indicates how long it takes until the item can be picked up by the player")]
    [SerializeField] private float _spawnCooldown = 1.0f;
    [SerializeField] private float _magnetSpeed = 5.0f;
    [SerializeField] private float _magnetRadius = 3.0f;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private SpriteRenderer _multiSpriteIndicator;

    private int stack;
    private ItemSO myItem;
    private Transform target;

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

        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
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
        if (target == null) return;
        if (collision.gameObject.tag != "Player") return;

        int stackCount = stack;
        for (int i = 0; i < stackCount; i++)
        {
            if (!GameManager.Instance.inventory.AddItem(myItem)) return;
            stack--;

            if(stack == 1) _multiSpriteIndicator.enabled = false;
        }

        Destroy(gameObject);
    }
}
