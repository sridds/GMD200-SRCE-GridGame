using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Tool_SO", menuName = "Items/Tool", order = 1)]
public class ToolSO : ItemSO
{
    public override ItemSO Clone() => CloneGeneric<ToolSO>();

    public override void OnUse(UseContext ctx) => OnUseDown(ctx);

    public override void OnUseDown(UseContext ctx)
    {
        if (ctx.raycast.collider.TryGetComponent<IBreakable>(out IBreakable breakable))
        {
            breakable.Damage(this);
        }
    }
}