using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Consumable_SO", menuName = "Items/Consumable", order = 1)]
public class ConsumableSO : ItemSO
{
    public int HealAmount;

    public override ItemSO Clone() => CloneGeneric<ConsumableSO>();

    public override void OnUse(UseContext ctx) {
        Debug.Log("fuck piss shit");
        ctx.mySlot.RemoveFromStack(1);
    }
}
