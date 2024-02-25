using UnityEngine;

[CreateAssetMenu(fileName = "Material_SO", menuName = "Items/Material", order = 1)]
public class MaterialSO : ItemSO
{
    [Tooltip("Indicates how many of this item can be stacked in the inventory")]
    public int MaxStack = 16;
}
