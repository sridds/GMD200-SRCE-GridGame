using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class TileManager : MonoBehaviour
{

    [Header("Map Settings")]

    [Tooltip("The x or width or the map generated")]
    [SerializeField] private int gridWidth;

    [Tooltip("The y or height of the map generated")]
    [SerializeField] private int gridHeight;

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

    [Tooltip("Array of Tile prefabs, if you want to add a new tile type drag it into here")]
    [SerializeField] private GameObject[] tilePrefabsArray;

    [Header("Sand Settings")]

    [Tooltip("The density of sand generated around water")]
    [SerializeField] private int sandBorderDensity = 2;

    [Tooltip("If Enabled, randomizes the density of each sand generation")]
    [SerializeField] private bool isRandomDensity;

    [ShowIf(nameof(isRandomDensity))]
    [SerializeField] private int randomMin = 1;

    [ShowIf(nameof(isRandomDensity))]
    [SerializeField] private int randomMax = 3;


    [Header("Water Settings")]

    [Range(0.3f, 1f)]
    [SerializeField] private float waterAmount;

    [SerializeField] private bool cleanRougeParticles;

    [ShowIf(nameof(cleanRougeParticles))]
    [Tooltip("The amount of tiles that need to be isolated to delete")]
    [SerializeField] private int rougeParticleThreshold = 4;

    [Header("Misc Settings")]

    [SerializeField] private LayerMask groundLayer;

    Dictionary<int, GameObject> tileset;

    Dictionary<int, GameObject> tilesetGroups;

    private GameObject[,] currentGrid;

    private const int MAX_RANDOM_RANGE = 99999;
    void Start()
    {
        //Randomize position on grid
        offsetX = Random.Range(0, MAX_RANDOM_RANGE);
        offsetY = Random.Range(0, MAX_RANDOM_RANGE);

        //Create Terrain
        GenerateDictionary();
        GenerateTileGroups();
        GenerateTerrain();

        //Clean up lone terrain particles
        if (cleanRougeParticles)
            CleanRougeParticles();

        GenerateSandTiles();
    }

    /// <summary>
    /// Populates dictionary with tile prefabs
    /// </summary>
    void GenerateDictionary()
    {
        //Initialize new dictionary
        tileset = new Dictionary<int, GameObject>();
        //Add Tiles to dictionary (Scalable)
        for (int i = 0; i < tilePrefabsArray.Length; i++)
            tileset.Add(i, tilePrefabsArray[i]);
    }
    /// <summary>
    /// Create different parent object for each category of terrain
    /// </summary>
    void GenerateTileGroups()
    {
        tilesetGroups = new Dictionary<int, GameObject>();

        //Create an empty parent for each category of terrain
        foreach (KeyValuePair<int, GameObject> prefabPair in tileset)
        {
            GameObject tileGroupInstance = new(prefabPair.Value.name);

            tileGroupInstance.transform.parent = gameObject.transform;

            tileGroupInstance.transform.localPosition = Vector3.zero;

            tilesetGroups.Add(prefabPair.Key, tileGroupInstance);
        }

    }
    /// <summary>
    /// When Called generates the terrain
    /// </summary>
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
    /// <summary>
    /// Takes an x and y position in the grid and returns a terrain id value
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
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
        scaledPerlin *= waterAmount;

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
        GameObject tilePrefab = tileset[tileID];
        
        //Set tile to correct parent group
        GameObject tileGroup = tilesetGroups[tileID];

        GameObject tile = Instantiate(tilePrefab, tileGroup.transform);

        tile.transform.localPosition = new Vector3(x, y, 0);

        //Rename to cordinates in matrix
        tile.name = $"Tile_{x}_{y}";

        //Set position
        currentGrid[x, y] = tile;
    }

    /// <summary>
    /// Iterates through grid of tiles to find water tiles
    /// </summary>
    void GenerateSandTiles()
    {

        //Iterate though matrix
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                //Check if current tile is water
                if (currentGrid[x, y].layer == 4)
                {
                    //Set random sizes for each beach
                    if (isRandomDensity)
                        sandBorderDensity = Random.Range(randomMin, randomMax);

                    //Adds to border amount
                    for (int sandIncrement = 0; sandIncrement <= sandBorderDensity; sandIncrement++)
                    {
                        CalculateSandBorder(x, y, sandIncrement);
                    }
                }

            }
        }
    }
    /// <summary>
    /// Calculates where to replace ground tiles with sand tiles
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="increment"></param>
    void CalculateSandBorder(int x, int y, int increment)
    {
        //Iterate from left to right and from up to down to get adjacent tiles
        for (int adjacentX = -increment; adjacentX <= increment; adjacentX++)
        {
            for (int adjacentY = -increment; adjacentY <= increment; adjacentY++)
            {
                int currentX = x + adjacentX;
                int currentY = y + adjacentY;

                //Out of bounds check
                if (InBounds(currentX, currentY))
                {
                    //If the current tile is ground, replace it
                    if (currentGrid[currentX, currentY].layer == 3)
                    {
                        Destroy(currentGrid[currentX, currentY]);
                        GenerateTile(2, currentX, currentY);
                    }
                }
            }
        }
    }
    /// <summary>
    /// Searches for lone terrain instances and sets to surrounding terrain
    /// </summary>
    void CleanRougeParticles()
    {
        //Iterate though matrix
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                //Check if current tile is water
                if (currentGrid[x, y].layer == 4)
                {
                    CalculateGroupTiles(x, y);
                }
            }
        }
    }
    void CalculateGroupTiles(int x, int y)
    {
        int tileCount = 0;
        int[] waterTilesX = new int[10];
        int[] waterTilesY = new int[10];

        //Iterate from left to right and from up to down to get adjacent tiles
        for (int adjacentX = -1; adjacentX <= 1; adjacentX++)
        {
            for (int adjacentY = -1; adjacentY <= 1; adjacentY++)
            {
                int currentX = x + adjacentX;
                int currentY = y + adjacentY;

                //Out of bounds check
                if (InBounds(currentX, currentY))
                {
                    //If the current tile is ground, replace it
                    if (currentGrid[currentX, currentY].layer == 4)
                    {
                        waterTilesX[tileCount] = currentX;
                        waterTilesY[tileCount] = currentY;
                        tileCount++;
                    }
                }
            }
        }

        //Replace tiles with ground tiles
        if (tileCount <= rougeParticleThreshold)
        {
            for (int i = 0; i < tileCount; i++)
            {
                int currentX = waterTilesX[i];
                int currentY = waterTilesY[i];

                Destroy(currentGrid[currentX, currentY]);
                GenerateTile(0, currentX, currentY);
            }
        }
    }
    /// <summary>
    /// Returns whether the input cordinates are in the arrays bounds
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    bool InBounds(int x, int y)
    {
        if (x < 0 || x >= gridWidth || y < 0 || y >= gridHeight)
            return false;
        else
            return true;
    }

    /// <summary>
    /// Returns the world position of a tile when given the position in the index
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Vector2 GetTileCordinate(int x, int y)
    {
        return currentGrid[x, y].transform.position;
    }

    public GameObject GetTile(int x, int y)
    {
        return currentGrid[x, y];
    }
}
