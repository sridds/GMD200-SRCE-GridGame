using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCollection : MonoBehaviour
{
    [Header("Resource Settings")]
    [SerializeField] private int maxResources = 3;
    public void CollectResource(int x, int y, TileType resourceType)
    {
        int amount = Random.Range(1, maxResources);
        GameManager.Instance.AddResource(resourceType, amount);
        TileGenerator.updateTile?.Invoke(x, y, TileType.Grass);
    }
}
