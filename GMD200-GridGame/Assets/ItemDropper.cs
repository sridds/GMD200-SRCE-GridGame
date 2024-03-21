using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    [SerializeField]
    private WeightedList<ItemSO> _itemDrops = new WeightedList<ItemSO>();

    [SerializeField]
    private ItemDrop _itemDropPrefab;

    [SerializeField]
    private int _defaultDropCount = 1;

    public void DropItem(int count) {

        for(int i = 0; i < count; i++)
        {
            ItemSO item = _itemDrops.GetRandom();

            // create drop
            ItemDrop drop = Instantiate(_itemDropPrefab, transform.position, Quaternion.identity);

            // initalize dropper with the new item
            drop.Init(GameManager.Instance.player.transform, item);
        }
    }

    public void DropItem() => DropItem(_defaultDropCount);
}
