using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutGridManager : MonoBehaviour
{
    public event Action<TutGridTile> TileSelected;

    public int numRows = 5;
    public int numColumns = 6;
    public float padding = 0.1f;

    [SerializeField] private TutGridTile tilePrefab;
    [SerializeField] private TextMeshProUGUI text;


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
                tile.tutGridManager = this;
                tile.gridCoords = new Vector2Int(x, y);
            }
        }
    }

    public void OnTileHoverEnter(TutGridTile gridTile)
    {
        text.text = gridTile.gridCoords.ToString();
    }

    public void OnTileHoverExit(TutGridTile gridTile)
    {
        text.text = "---";
    }

    public void OnTileSelected(TutGridTile gridTile)
    {
        TileSelected?.Invoke(gridTile);
    }
}
