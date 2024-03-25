using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class ItemDropper : MonoBehaviour
{
    [System.Serializable]
    public struct RequiredDrop
    {
        public int count;
        public ItemSO item;
    }

    [SerializeField]
    private WeightedList<ItemSO> _itemDrops = new WeightedList<ItemSO>();

    [Tooltip("The list of items that must drop. Ignores count and drops exactly as the required drop specifies")]
    [SerializeField]
    private List<RequiredDrop> _requiredDrops = new List<RequiredDrop>();

    [SerializeField]
    private int _defaultDropCount = 1;

    [SerializeField]
    private bool _randomizeDropCount = false;

    [ShowIf(nameof(_randomizeDropCount))]
    [SerializeField]
    private int _maxDefaultDropCount = 1;

    [SerializeField]
    private int _allowedDropCalls = 1;

    int dropCalls = 0;

    private void Start()
    {
        GameManager.Instance.OnDayUpdate += ResetDropCalls;
    }

    private void ResetDropCalls() => dropCalls = 0;

    public void DropItem(int count) {
        // Drop random items
        for (int i = 0; i < count; i++)
        {
            ItemSO item = _itemDrops.GetRandom();

            // create drop
            ItemDrop drop = Instantiate(GameAssets.Instance.ItemDropPrefab, transform.position, Quaternion.identity);

            // initalize dropper with the new item
            drop.Init(GameManager.Instance.player.transform, item);
        }

        // Drop required items
        foreach (RequiredDrop required in _requiredDrops)
        {
            for(int i = 0; i < required.count; i++)
            {
                // create drop
                ItemDrop drop = Instantiate(GameAssets.Instance.ItemDropPrefab, transform.position, Quaternion.identity);

                // initalize dropper with the new item
                drop.Init(GameManager.Instance.player.transform, required.item);
            }
        }
    }

    public void DropItem()
    {
        dropCalls++;

        if (dropCalls > _allowedDropCalls) return;
        int count = _randomizeDropCount ? Random.Range(_defaultDropCount, _maxDefaultDropCount) : _defaultDropCount;

        DropItem(count);
    }
}
