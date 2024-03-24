using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Consumable_SO", menuName = "Items/Consumable", order = 1)]
public class ConsumableSO : ItemSO
{
    public int HealAmount;
    public int SaturationAmount;
    private float nextUseTime;

    public override ItemSO Clone() => CloneGeneric<ConsumableSO>();

    Hunger hungerInstance;

    public override void OnUse(UseContext ctx) {
        if (Time.time < nextUseTime) return;
        if (hungerInstance == null) hungerInstance = FindObjectOfType<Hunger>();

        // dont surpass current hunger
        if (hungerInstance.myStat.CurrentValue == hungerInstance.myStat.MaxValue) return;

        // remove from stack
        ctx.mySlot.RemoveFromStack(1);
        AudioHandler.instance.ProcessAudioData(hungerInstance.transform, "eat");

        // increase stats
        hungerInstance.IncreaseStat(HealAmount);
        hungerInstance.IncreaseSaturation(SaturationAmount);

        nextUseTime = UseCooldown + Time.time;
    }
}
