using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Weapon_SO", menuName = "Items/Weapon", order = 1)]
public class WeaponSO : ItemSO
{
    [Header("Stats")]
    [Min(1)]
    public int Attack;
    public DurabilityData Durability;

    public override ItemSO Clone() => CloneGeneric<WeaponSO>();
}
