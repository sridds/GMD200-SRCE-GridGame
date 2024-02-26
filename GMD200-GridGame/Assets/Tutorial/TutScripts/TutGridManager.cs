using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutGridManager : MonoBehaviour
{
    public int numRows = 5;
    public int numColumns = 6;
    public float padding = 0.1f;

    [SerializeField] private TutGridTile tilePrefab;


    private void Awake()
    {
        InItGrid();
    }

    private void InItGrid()
    {
        for (int y = 0; y < numRows; y++)
        {
            for (int x = 0; x < numColumns; x++)
            {
                TutGridTile tile = Instantiate(tilePrefab, transform);
                Vector2 tilePos = new Vector2(x + (padding*x), y + (padding*y));
                tile.transform.position = tilePos;
                tile.name = $"Tile_{x}_{y}";
            }
        }
    }
}
