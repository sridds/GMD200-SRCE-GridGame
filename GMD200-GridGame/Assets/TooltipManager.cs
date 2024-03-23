using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TooltipData
{
    public string headerText;
    public string subheaderText;
    public Sprite icon;
}

public class TooltipManager : MonoBehaviour
{
    [SerializeField]
    private Tooltip tooltip;

    Queue<TooltipData> datas = new Queue<TooltipData>();

    private void Update()
    {
        if (!tooltip.IsReady) return;
        if (datas.Count == 0) return;

        // dequeue and initialize
        tooltip.Initialize(datas.Dequeue());
    }

    public void CreateTooltip(TooltipData tooltip) => datas.Enqueue(tooltip);
}
