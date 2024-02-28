using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TileType
{
    Ground,
    Water,
    Sand,
}
public class TileData
{
    public TileType tileType;
    public Vector2 tilePosition;

    public TileData()
    {
        tileType = TileType.Ground;
        tilePosition = Vector2.zero;
    }
    public TileData(TileType tileType, Vector2 tilePosition)
    {
        this.tileType = tileType;
        this.tilePosition = tilePosition;
    }
}
