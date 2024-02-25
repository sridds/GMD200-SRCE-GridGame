using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe_SO", menuName = "Items/Item Recipe", order = 1)]
public class RecipeSO : ScriptableObject
{
    [Tooltip("A list of the required materials to produce the output item")]
    public List<RecipeResource> RequiredResources = new List<RecipeResource>();

    [Tooltip("The output item that the resources create")]
    public ItemSO OutputItem;

    [Min(1)]
    public int OutputAmount = 1;
}

[System.Serializable]
public struct RecipeResource
{
    public MaterialSO Material;

    [Min(1)]
    public int Amount;
}
