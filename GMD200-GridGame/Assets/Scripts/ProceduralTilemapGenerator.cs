using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using NaughtyAttributes;

public class ProceduralTilemapGenerator : MonoBehaviour
{
    [Header("Map Settings")]

    [Tooltip("The x or width or the map generated")]
    [SerializeField] private int gridWidth = 9;

    [Tooltip("The y or height of the map generated")]
    [SerializeField] private int gridHeight = 9;

    [Tooltip("If Enabled, will generate a random position on load")]
    [SerializeField] private bool randomOffset = true;

    [HideIf(nameof(randomOffset))]
    [Tooltip("Offset the generated tiles on the x-axis")]
    [SerializeField] private int offsetX;

    [HideIf(nameof(randomOffset))]
    [Tooltip("Offset the generated tiles on the y-axis")]
    [SerializeField] private int offsetY;

    [Tooltip("Increases or decreses the size of the generation on the grid (Reccomended: Between 4 - 20)")]
    [SerializeField] private float magnification = 7f;

    [SerializeField] private TileBase[] tilesArray;

    public Tilemap Tilemap { get; private set; }

    Dictionary<int, TileBase> tileset;

    void Start()
    {
        Tilemap = GetComponentInChildren<Tilemap>();

        GenerateDictionary();
        GenerateTerrain();
    }

    private void GenerateDictionary()
    {
        //Initialize new dictionary
        tileset = new Dictionary<int, TileBase>();
        //Add Tiles to dictionary (Scalable)
        for (int i = 0; i < tilesArray.Length; i++)
            tileset.Add(i, tilesArray[i]);
    }

    /// <summary>
    /// When Called generates the terrain
    /// </summary>
    void GenerateTerrain()
    {
        //Initialize grid
        //perlinNoiseMap = new int[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                int tileID = GeneratePerlinNoise(x, y);

                Debug.Log(tileID);

                GenerateTile(x, y, tileID);
            }
        }
    }
    int GeneratePerlinNoise(int x, int y)
    {
        //Calculate perlin x and y values
        float perlinX = (x - offsetX) / magnification;

        float perlinY = (y - offsetY) / magnification;

        float rawPerlin = Mathf.PerlinNoise(perlinX, perlinY);

        //Normalize perlin value
        float clampedPerlin = Mathf.Clamp01(rawPerlin);

        //Scale it by number of entries in dictionary
        float scaledPerlin = clampedPerlin * tileset.Count;

        //Water needs to be second in the array for this to work
        //scaledPerlin *= waterAmount;

        //If number out of bounds comes up, set it to highest value in dictionary
        if (scaledPerlin == tileset.Count)
            scaledPerlin = tileset.Count - 1;


        return Mathf.FloorToInt(scaledPerlin);
    }

    /// <summary>
    /// Instantiate each tile at specified cordinates 
    /// </summary>
    /// <param name="tileID"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    void GenerateTile(int tileID, int x, int y)
    {
        //Create the correct tile prefab from dictionary
        TileBase tileInstance = tileset[tileID];

        //Create tile on tilemap
        Tilemap.SetTile(new Vector3Int(x, y, 0), tileInstance);
    }
}