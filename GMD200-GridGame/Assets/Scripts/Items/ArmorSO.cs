using UnityEngine;

[CreateAssetMenu(fileName = "Armor_SO", menuName = "Items/Armor", order = 1)]
public class ArmorSO : ItemSO
{
    // Durability settings
    public DurabilityData Durability;

    public override ItemSO Clone()
    {
        ArmorSO Instance = ScriptableObject.CreateInstance<ArmorSO>();
        Instance.name = ItemName;

        return CopyValuesReflection(Instance);
    }
}
