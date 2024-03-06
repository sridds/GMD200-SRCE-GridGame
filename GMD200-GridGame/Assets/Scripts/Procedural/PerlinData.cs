using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class PerlinData : MonoBehaviour
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

    [Header("Resource Settings")]

    [Tooltip("The amount of resources that will spawn")]
    [Range(0, 100)]
    [SerializeField] private int resourceRarity = 25;

    [Tooltip("The amount of hitpoints resources will have")]
    [SerializeField] private int resourceHitpoints = 3;

    [Header("Sand Settings")]

    [Tooltip("The density of sand generated around water")]
    [SerializeField] private int sandBorderDensity = 1;

    [Header("Water Settings")]

    [Range(0.3f, 1f)]
    [SerializeField] private float waterAmount;

    [SerializeField] private bool cleanRogueParticles;

    [ShowIf(nameof(cleanRogueParticles))]
    [Tooltip("The amount of tiles that need to be isolated to delete")]
    [SerializeField] private int rogueParticleThreshold = 4;

    public TileData[,] tiles;

    private const int 
        MAX_RANDOM_RANGE = 99999,
        PREFAB_COUNT = 3;
    public int _gridWidth { get { return gridWidth; } }
    public int _gridHeight { get { return gridHeight; } }

    public static PerlinData Instance { get; private set; }

    void Awake()
    {
        //Singleton
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        tiles = new TileData[gridWidth, gridHeight];

        //Set random position of perlin noise
        if (randomOffset)
        {
            offsetX = Random.Range(0, MAX_RANDOM_RANGE);
            offsetY = Random.Range(0, MAX_RANDOM_RANGE);
        }

        GenerateTileData();
        FindWaterTiles();
        GenerateResources();
    }
    /// <summary>
    /// Generates a new grid and calls the TileGenerator to update the physical grid
    /// </summary>
    public void GenerateNewGrid(int width, int height)
    {
        gridWidth = width;
        gridHeight = height;

        tiles = new TileData[gridWidth, gridHeight];

        //Set random position of perlin noise
        if (randomOffset)
        {
            offsetX = Random.Range(0, MAX_RANDOM_RANGE);
            offsetY = Random.Range(0, MAX_RANDOM_RANGE);
        }

        GenerateTileData();
        FindWaterTiles();
        GenerateResources();
        TileGenerator.updateGrid?.Invoke(gridWidth, gridHeight);
    }
    /// <summary>
    /// Populate TileData with a perlin noise and store position in Tile data
    /// </summary>
    void GenerateTileData()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                //User Adjustments
                float adjustmentX = (x - offsetX) / magnification;
                float adjustmentY = (y - offsetY) / magnification;

                //Get perlin value
                float rawPerlin = Mathf.PerlinNoise(adjustmentX, adjustmentY);

                //Normalize perlin value between 0-1
                float clampedPerlin = Mathf.Clamp01(rawPerlin);

                //Scale perlin by number of tiles
                float scaledPerlin = clampedPerlin * PREFAB_COUNT;

                //Amount of water spawned during generation
                scaledPerlin *= waterAmount;

                //Prevent rare cases of numbers outside bounds
                if (scaledPerlin >= PREFAB_COUNT - 1)
                    scaledPerlin = PREFAB_COUNT - 2;

                //Set TileData values
                tiles[x, y] = new TileData((TileType)Mathf.FloorToInt(scaledPerlin), new Vector2(x, y), resourceHitpoints);
            }
        }
    }
    /// <summary>
    /// Iterates and finds water tiles
    /// </summary>
    void FindWaterTiles()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (cleanRogueParticles)
                    CleanRogueParticles(x, y);

                if (tiles[x, y].tileType == TileType.Water)
                {
                    GenerateSandTiles(x, y);
                }
            }
        }
    }
    /// <summary>
    /// Replaces Neighbor ground tiles with sand tiles
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    void GenerateSandTiles(int x, int y)
    {
        //Find neighbor tiles
        List<TileData> neighborTiles = StoreNeighborTiles(x, y, sandBorderDensity);

        //Replace ground tiles with sand
        for (int i = 0; i < neighborTiles.Count; i++)
        {
            if (neighborTiles[i].tileType == TileType.Grass)
                neighborTiles[i].tileType = TileType.Sand;   
        }   
    }
    /// <summary>
    /// Randomly generates resources on the map
    /// </summary>
    void GenerateResources()
    {
        int spawnChance;
        TileType resourceType = TileType.Tree;
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (tiles[x, y].tileType == TileType.Grass)
                {
                    //Determine whether resource will spawn on this tile
                    spawnChance = Random.Range(0, 1000);
                    if (spawnChance <= resourceRarity)
                    {
                        //Range of resource values
                        resourceType = resourceType == TileType.Tree ? TileType.Rock : TileType.Tree;
                        tiles[x, y].tileType = resourceType;
                    }
                }
            }
        }
    }
    /// <summary>
    /// Cleans up solitary water instances
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    void CleanRogueParticles(int x, int y)
    {
        List<TileData> particleList = new();

        List<TileData> neighborTiles = StoreNeighborTiles(x, y, rogueParticleThreshold);

        for (int i = 0; i < neighborTiles.Count; i++)
        {
            //Add particles to list
            if (neighborTiles[i].tileType == TileType.Water)
                particleList.Add(tiles[x, y]);
        }

        //If the amount of solitary water particles is less than threshold
        //Update tiles to ground type
        if (particleList.Count <= rogueParticleThreshold)
        {
            for (int j = 0; j < particleList.Count; j++)
                particleList[j].tileType = TileType.Grass;
        }
    }

    /// <summary>
    /// Returns the neighboring tiles in a determined size
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    List<TileData> StoreNeighborTiles(int x, int y, int size)
    {
        //Find the area of the space being searched
        float rawArea = Mathf.Pow(size * 3, 2);
        int area = Mathf.FloorToInt(rawArea);
        int index = 0;

        List<TileData> neighborTiles = new();
        
        //Iterate from left to right and from up to down to get adjacent tiles
        for (int adjacentX = -size; adjacentX <= size; adjacentX++)
        {
            for (int adjacentY = -size; adjacentY <= size; adjacentY++)
            {
                index++;
                if (index == area)
                    index = area - 1;

                int currentX = x + adjacentX;
                int currentY = y + adjacentY;

                if (InBounds(currentX, currentY))
                {
                    neighborTiles.Add(tiles[currentX, currentY]);
                }
            }
        }
        return neighborTiles;
    }

    /// <summary>
    /// Takes a cordinate and determines whether it is on an edge
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public bool InBounds(int x, int y)
    {
        if (x < 0 || x >= gridWidth || y < 0 || y >= gridHeight)
            return false;
        else
            return true;
    }
}
