using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutGridTile : MonoBehaviour
{
    public TutGridManager tutGridManager;

    public Vector2Int gridCoords;

    private SpriteRenderer spriteRenderer;
    private Color defaultColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
    }

    private void OnMouseOver()
    {
        tutGridManager.OnTileHoverEnter(this);
        SetColor(Color.gray);
    }

    private void OnMouseExit()
    {
        tutGridManager.OnTileHoverExit(this);
        ResetColor();
    }

    private void OnMouseDown()
    {
        tutGridManager.OnTileSelected(this);
    }

    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }

    public void ResetColor()
    {
        spriteRenderer.color = defaultColor;
    }
}
