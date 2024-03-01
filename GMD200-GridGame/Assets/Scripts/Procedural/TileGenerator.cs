using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PerlinData))]
public class TileGenerator : MonoBehaviour
{
    private PerlinData data;

    public GameObject[,] currentGrid;

    [Header("Terrain Generator Settings")]

    [Tooltip("Array of Tile prefabs, if you want to add a new tile type drag it into here")]
    [SerializeField] private GameObject[] tilePrefabs;

    Dictionary<TileType, GameObject> tileset;

    Dictionary<TileType, GameObject> tilesetGroups;

    // Start is called before the first frame update
    void Start()
    {
        data = PerlinData.Instance;
        currentGrid = new GameObject[data._gridWidth, data._gridHeight];

        PopulateDictionary();
        GenerateTileGroups();
        PopulateGrid();
    }
    /// <summary>
    /// Populates dictionary with tile prefabs
    /// </summary>
    void PopulateDictionary()
    {
        tileset = new Dictionary<TileType, GameObject>();
        for (int i = 0; i < tilePrefabs.Length; i++)
        {
            tileset.Add((TileType)i, tilePrefabs[i]);
        }
    }
    /// <summary>
    /// Create different parent object for each category of terrain
    /// </summary>
    void GenerateTileGroups()
    {
        tilesetGroups = new Dictionary<TileType, GameObject>();

        //Create an empty parent for each category of terrain
        foreach (KeyValuePair<TileType, GameObject> prefabPair in tileset)
        {
            GameObject tileGroupInstance = new(prefabPair.Value.name);

            tileGroupInstance.transform.parent = gameObject.transform;

            tileGroupInstance.transform.localPosition = Vector3.zero;

            tilesetGroups.Add(prefabPair.Key, tileGroupInstance);
        }
    }
    /// <summary>
    /// Populate gamespace grid with gameobjects 
    /// </summary>
    void PopulateGrid()
    {
        for (int x = 0; x < data._gridWidth; x++)
        {
            for (int y = 0; y < data._gridHeight; y++)
            {
                InstantiateTile(x, y);
            }
        }
    }
    /// <summary>
    /// Creates a tile based on perlin data at specified location
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    void InstantiateTile(int x, int y)
    {
        TileData tileData = data.tiles[x, y];

        GameObject tilePrefab = tileset[tileData.tileType];

        GameObject tileGroup = tilesetGroups[tileData.tileType];

        //Create object
        GameObject tileInstance = Instantiate(tilePrefab, tileGroup.transform);

        tileInstance.transform.localPosition = tileData.tilePosition;

        //Change name to cordinates 
        tileInstance.name = $"{tileData.tileType}:_{x}_{y}";

        //Set object in grid
        currentGrid[x, y] = tileInstance;
    }
    /// <summary>
    /// Takes a tiles position and updates its information
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    void UpdateTile(int x, int y)
    {
        Destroy(currentGrid[x, y]);

        InstantiateTile(x, y);    
    }
    /// <summary>
    /// Takes a cordinate on the grid and changes the data's tile type (Refreshes after, may cause lag)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="newTileType"></param>
    public void UpdateTileInfo(int x, int y, TileType newTileType)
    {
        data.tiles[x, y].tileType = newTileType;
        UpdateTile(x, y);
    }
    /// <summary>
    /// Takes a cordinate on the grid and changes the data's position (Refreshes after, may cause lag)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="newTilePosition"></param>
    public void UpdateTileInfo(int x, int y, Vector2 newTilePosition)
    {
        data.tiles[x, y].tilePosition = newTilePosition;
        UpdateTile(x, y);
    }
}
