using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TileType
{
    Grass,
    Water,
    Sand,
}
public class TileData
{
    public TileType tileType;
    public Vector2 tilePosition;
    public int hitPoints;

    public ResourceSO resource;
    public GameObject resourceInstance;

    public TileData(TileType tileType, Vector2 tilePosition, int hitPoints)
    {
        this.tileType = tileType;
        this.tilePosition = tilePosition;
        this.hitPoints = hitPoints;
        resource = null;
    }
}
