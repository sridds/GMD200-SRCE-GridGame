using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCollection : MonoBehaviour
{
    [Header("Resource Settings")]
    [SerializeField] private int maxResources = 3;
    public void CollectResource(int x, int y, TileData tileData)
    {
        //Take away hitpoint from resource
        tileData.hitPoints--;

        if (tileData.hitPoints > 0) return;

        int amount = Random.Range(1, maxResources);
        GameManager.Instance.AddResource(tileData.tileType, amount);
        TileGenerator.updateTile?.Invoke(x, y, TileType.Grass);
    }
}
