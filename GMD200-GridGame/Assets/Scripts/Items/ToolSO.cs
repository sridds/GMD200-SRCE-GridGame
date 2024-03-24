using UnityEngine;
using System.Collections.Generic;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Tool_SO", menuName = "Items/Tool", order = 1)]
public class ToolSO : ItemSO
{
    [Header("Use Settings")]
    public float UseCooldown;

    private float nextUseTime;

    public override ItemSO Clone() => CloneGeneric<ToolSO>();

    public override void OnUse(UseContext ctx)
    {
        if (Time.time < nextUseTime) return;
        if (ctx.raycast.collider == null) return;

        if (ctx.raycast.collider.TryGetComponent<IBreakable>(out IBreakable breakable))
        {
            if(breakable.Damage(this)) CurrentDurability -= 2;

            if(CurrentDurability <= 0) {
                AudioHandler.instance.ProcessAudioData(ctx.raycast.collider.transform, "tool_break");
                ctx.mySlot.ResetSlot();
            }
        }

        nextUseTime = UseCooldown + Time.time;
    }
}