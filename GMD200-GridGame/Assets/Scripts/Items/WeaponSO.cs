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

    public override void OnUse(UseContext ctx) => OnUseDown(ctx);

    public override void OnUseDown(UseContext ctx)
    {
        // don't try breakign it
        if (ctx.raycast.collider.TryGetComponent<IBreakable>(out IBreakable breakable)) return;

        if (ctx.raycast.collider.TryGetComponent<Health>(out Health health))
        {
            health.TakeDamage(Attack);
        }
    }
}
