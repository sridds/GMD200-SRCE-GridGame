using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Listing", menuName = "Shop/Shop Listing")]
public class ShopListing : ScriptableObject
{
    public ItemSO Listing;
    public int Price;

    [MinMaxSlider(0, 100)]
    public Vector2Int SalePercentage;

    [Range(0, 100)]
    public int SaleChance;
}