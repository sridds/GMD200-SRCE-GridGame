using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Catalogue", menuName = "Shop/Catalogue")]
public class ShopCatalogue : ScriptableObject
{
    // This list contains every possible listing for the catalogue. Replace this later with a weighted list
    public List<ShopListing> Listings = new();

    // the maximum amount of items a shop can have
    public int MaxShopItems;
}
