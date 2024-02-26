using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Tool_SO", menuName = "Items/Tool", order = 1)]
public class ToolSO : ItemSO
{
    public List<BreakableData> Breakables;
    public DurabilityData Durability;

    public override ItemSO Clone() => CloneGeneric<ToolSO>();
}

[System.Serializable]
public struct BreakableData
{
    // All tiles should have a tile tier which can be compared with the breakable data
    public int Tier;

    // Certain tiers may be slower than others to break
    public int RequiredHits;
}
