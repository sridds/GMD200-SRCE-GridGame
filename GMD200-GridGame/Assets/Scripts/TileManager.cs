using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{

    [Header("Map Settings")]
    [Tooltip("The x or width or the map generated")]
    [SerializeField] private int gridWidth= 9;

    [Tooltip("The y or height of the map generated")]
    [SerializeField] private int gridHeight = 9;

    [SerializeField] private GameObject[] tileArray;

    [SerializeField] private GameObject parentObject;

    private GameObject[,] currentGrid;

    Dictionary<int, GameObject> tileset;

    void Start()
    {
        GenerateDictionary();
        GenerateTerrain();
    }

    /// <summary>
    /// Populates dictionary with tile prefabs
    /// </summary>
    void GenerateDictionary()
    {
        tileset = new Dictionary<int, GameObject>();
        //Add Tiles to dictionary
        for (int i = 0; i < tileArray.Length; i++)
            tileset.Add(i, tileArray[i]);
    }

    void GenerateTerrain()
    {
        //Initialize grid
        currentGrid = new GameObject[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                int tileID = GeneratePerlinNoise(x, y);

                GenerateTile(tileID, x, y);
            }
        }
    }

    int GeneratePerlinNoise(int x, int y)
    {
        float rawPerlin = Mathf.PerlinNoise(x, y);

        float clampedPerlin = Mathf.Clamp01(rawPerlin);

        float scaledPerlin = clampedPerlin * tileset.Count;
        
        //If number out of bounds comes up, set it to highest value in dictionary
        if (scaledPerlin == tileset.Count)
            scaledPerlin = tileset.Count - 1;

        return Mathf.FloorToInt(scaledPerlin);
    }

    void GenerateTile(int tileID, int x, int y)
    {
        GameObject tilePrefab = tileset[tileID];

        GameObject tile = Instantiate(tilePrefab, parentObject.transform);
        tile.transform.localPosition = new Vector3(x, y, 0);

        tile.transform.parent = parentObject.transform;


        currentGrid[x, y] = tile;
    }
}
