using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandManager : MonoBehaviour
{

    [Header("Island Iteration Settings")]

    [Tooltip("The amount of water on the map as the days progress")]
    [SerializeField] private AnimationCurve waterLevel;

    //Should be run every day before generation

    /// <summary>
    /// Updates generation values with new settings
    /// </summary>
    void NewMapSettings()
    {
        PerlinData.Instance.SetWaterLevel(waterLevel.Evaluate(GameManager.Instance.day));
    }
}
