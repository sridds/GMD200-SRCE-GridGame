using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Weapon_SO", menuName = "Items/Weapon", order = 1)]
public class WeaponSO : ItemSO
{
    [Header("Stats")]
    [Min(1)]
    public int Attack;

    public float KnockbackStrength;
    private float nextUseTime;

    public override ItemSO Clone() => CloneGeneric<WeaponSO>();

    public override void OnUse(UseContext ctx)
    {
        if (Time.time < nextUseTime) return;

        if (ctx.raycast.collider == null) return;
        if (ctx.raycast.collider.TryGetComponent<IBreakable>(out IBreakable breakable)) return;

        // decrease health
        if (ctx.raycast.collider.TryGetComponent<Health>(out Health health)) health.DecreaseStat(Attack);
        // apply knockback
        if (ctx.raycast.collider.TryGetComponent<KnockbackHandler>(out KnockbackHandler knockback)) knockback.ApplyKnockback(GameManager.Instance.player, KnockbackStrength);

        nextUseTime = UseCooldown + Time.time;
    }
}
