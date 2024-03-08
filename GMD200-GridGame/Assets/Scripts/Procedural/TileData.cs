using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TileType
{
    Grass,
    Water,
    Sand,
    Tree,
    Rock,
}
public enum ResourceType
{

}
public class TileData
{
    public TileType tileType;
    public Vector2 tilePosition;
    public int hitPoints;

    public TileData(TileType tileType, Vector2 tilePosition, int hitPoints)
    {
        this.tileType = tileType;
        this.tilePosition = tilePosition;
        this.hitPoints = hitPoints;
    }
}
