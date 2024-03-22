using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Weapon_SO", menuName = "Items/Weapon", order = 1)]
public class WeaponSO : ItemSO
{
    [Header("Stats")]
    [Min(1)]
    public int Attack;

    public override ItemSO Clone() => CloneGeneric<WeaponSO>();

    public override void OnUse(UseContext ctx)
    {
        if (ctx.raycast.collider == null) return;

        // don't try breakign it
        if (ctx.raycast.collider.TryGetComponent<IBreakable>(out IBreakable breakable)) return;

        if (ctx.raycast.collider.TryGetComponent<Health>(out Health health))
        {
            health.TakeDamage(Attack);
        }
    }
}
