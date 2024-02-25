using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Tool_SO", menuName = "Items/Tool", order = 1)]
public class ToolSO : ItemSO
{
    // In the future, this will be replaced with the corresponding class that this will break. Tiles will fill this list and be compared
    public List<GameObject> Breakables;
}
