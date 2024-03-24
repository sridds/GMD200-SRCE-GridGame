using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Weapon_SO", menuName = "Items/Weapon", order = 1)]
public class WeaponSO : ItemSO
{
    [Header("Stats")]
    [Min(1)]
    public int Attack;
    public float KnockbackStrength;
    public bool CanUseAsTool;

    [Header("Use Settings")]
    public float UseCooldown;

    private float nextUseTime;

    public override ItemSO Clone() => CloneGeneric<WeaponSO>();

    public override void OnUse(UseContext ctx)
    {
        if (Time.time < nextUseTime) return;

        if (ctx.raycast.collider == null) return;

        if (ctx.raycast.collider.TryGetComponent<IBreakable>(out IBreakable breakable)) {

            // only works if can use as tool
            if (!CanUseAsTool) return;

            // durabilty
            if(breakable.Damage(this)) DecrementDurability(ctx);
        }

        // decrease health
        else if (ctx.raycast.collider.TryGetComponent<Health>(out Health health)) {
            health.DecreaseStat(Attack);
            DecrementDurability(ctx);
        }
        // apply knockback
        if (ctx.raycast.collider.TryGetComponent<KnockbackHandler>(out KnockbackHandler knockback)) knockback.ApplyKnockback(GameManager.Instance.player, KnockbackStrength);

        nextUseTime = UseCooldown + Time.time;
    }

    private void DecrementDurability(UseContext ctx)
    {
        CurrentDurability -= 2;

        if (CurrentDurability <= 0)
        {
            AudioHandler.instance.ProcessAudioData(ctx.raycast.collider.transform, "tool_break");
            ctx.mySlot.ResetSlot();
        }
    }
}
