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


    private int[,] currentGrid;

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
        //Add Tiles to dictionary
        for (int i = 0; i < tileArray.Length; i++)
            tileset.Add(i, tileArray[i]);
    }

    void GenerateTerrain()
    {
        //Initialize grid
        currentGrid = new int[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                //currentGrid[x, y] = GeneratePerlinNoise(x, y);
            }
        }
    }

   /* int GeneratePerlinNoise(int x, int y)
    {
        float rawPerlin = Mathf.PerlinNoise(x, y);

    }*/
}
