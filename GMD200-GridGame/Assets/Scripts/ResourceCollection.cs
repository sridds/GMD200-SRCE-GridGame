using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCollection : MonoBehaviour
{
    public void CollectResource(int x, int y, TileType resourceType)
    {
        //Add to inventory
        TileGenerator.updateTile?.Invoke(x, y, TileType.Grass);
    }
}
