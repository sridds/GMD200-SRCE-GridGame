using UnityEngine;
using System.Collections.Generic;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Tool_SO", menuName = "Items/Tool", order = 1)]
public class ToolSO : ItemSO
{
    public override ItemSO Clone() => CloneGeneric<ToolSO>();

    public override void OnUse(UseContext ctx)
    {
        if (ctx.raycast.collider == null) return;

        if (ctx.raycast.collider.TryGetComponent<IBreakable>(out IBreakable breakable))
        {
            breakable.Damage(this);
            CurrentDurability -= 2;

            if(CurrentDurability <= 0) {
                ctx.mySlot.ResetSlot();
            }
        }
    }
}