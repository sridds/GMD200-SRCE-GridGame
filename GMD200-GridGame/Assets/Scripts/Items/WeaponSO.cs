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

    public override void OnUse(UseContext ctx) { }

    public override void OnUseDown(UseContext ctx) { }
}

public struct UseContext
{
    public RaycastHit2D raycast;

    public UseContext(RaycastHit2D raycast)
    {
        this.raycast = raycast;
    }
}
