using UnityEngine;

[CreateAssetMenu(fileName = "Weapon_SO", menuName = "Items/Weapon", order = 1)]
public class WeaponSO : ItemSO
{
    public DurabilityData Durability;

    public override ItemSO Clone() => CloneGeneric<WeaponSO>();
}
