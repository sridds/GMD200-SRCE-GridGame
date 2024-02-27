using UnityEngine;

[CreateAssetMenu(fileName = "Armor_SO", menuName = "Items/Armor", order = 1)]
public class ArmorSO : ItemSO
{
    [Header("Stats")]
    [Min(1)]
    public int Defence;

    // Durability settings
    public DurabilityData Durability;

    public override ItemSO Clone() => CloneGeneric<ArmorSO>();
}
