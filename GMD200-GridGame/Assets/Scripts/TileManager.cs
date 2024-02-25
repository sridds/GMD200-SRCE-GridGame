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

    [Tooltip("Offset the generated tiles on the x-axis")]
    [SerializeField] private int offsetX;

    [Tooltip("Offset the generated tiles on the y-axis")]
    [SerializeField] private int offsetY;

    [Tooltip("Increases or decreses the size of the generation on the grid (Reccomended: Between 4 - 20)")]
    [SerializeField] private float magnification = 7f;

    [Tooltip("Array of Tile prefabs, if you want to add a new tile type drag it into here")]
    [SerializeField] private GameObject[] tilePrefabsArray;

    [Header("Sand Settings")]

    [SerializeField] private GameObject sandTile;

    [SerializeField] private int sandBorderDensity = 2;


    [Header("Water Settings")]

    [Range(0.5f, 1f)]
    [SerializeField] private float waterAmount;

    [SerializeField] private bool cleanRougeParticles;

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

        GenerateDictionary();
        GenerateTileGroups();
        GenerateTerrain();
        //GenerateSandTiles();

        if (cleanRougeParticles)
            CleanRougeParticles();
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
        foreach(KeyValuePair<int, GameObject> prefabPair in tileset)
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
        GameObject tilePrefab = tileset[tileID];

        GameObject tileGroup = tilesetGroups[tileID];

        GameObject tile = Instantiate(tilePrefab, tileGroup.transform);

        tile.transform.localPosition = new Vector3(x, y, 0);

        tile.name = $"Tile_{x}_{y}";
        
        currentGrid[x, y] = tile;
    }

    void GetCordinate(int x, int y)
    {

    }

    /// <summary>
    /// Search for water tiles, and if surrounding tiles are grass, replace with sand
    /// </summary>
   /* void GenerateSandTiles()
     {
        for (int rows = 0; rows < gridWidth; rows++)
        {
            for (int columns = 0; columns < gridHeight; columns++)
            {
                if (currentGrid[rows, columns].layer == 4)
                {
                    for (int sandIncrement = 1; sandIncrement <= sandBorderDensity; sandIncrement++)
                    {
                        if (IsEdge(rows, columns, sandIncrement))
                        {
                            Debug.Log("sand Placed");

                            if (currentGrid[rows + sandIncrement, columns + sandIncrement].layer == groundLayer)
                                currentGrid[rows + sandIncrement, columns + sandIncrement] = sandTile;

                            if (currentGrid[rows - sandIncrement, columns + sandIncrement].layer == groundLayer)
                                currentGrid[rows - sandIncrement, columns + sandIncrement] = sandTile;

                            if (currentGrid[rows + sandIncrement, columns - sandIncrement].layer == groundLayer)
                                currentGrid[rows + sandIncrement, columns - sandIncrement] = sandTile;

                            if (currentGrid[rows - sandIncrement, columns - sandIncrement].layer == groundLayer)
                                currentGrid[rows - sandIncrement, columns - sandIncrement] = sandTile;
                        }

                        
                    }
                }
                
            }
        }
     }

    bool IsEdge(int x, int y, int increment)
    {
        if (x == 0 || y == 0)
            return false;

        return true;
    }*/

    void CleanRougeParticles()
    {

    }
}
