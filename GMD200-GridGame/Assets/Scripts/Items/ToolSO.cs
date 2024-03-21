using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Tool_SO", menuName = "Items/Tool", order = 1)]
public class ToolSO : ItemSO
{
    public DurabilityData Durability;

    public override ItemSO Clone() => CloneGeneric<ToolSO>();
}