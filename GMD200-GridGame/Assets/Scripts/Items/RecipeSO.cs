using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe_SO", menuName = "Items/Item Recipe", order = 1)]
public class RecipeSO : ScriptableObject
{
    public List<MaterialSO> RequiredResources = new List<MaterialSO>();

    public ItemSO OutputItem;
}
