using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Thanks to code monkey for this tutorial
/// </summary>
public class GenericGrid<TGridObject>
{
    public event EventHandler<OnGridObjectChangedArgs> OnGridObjectChanged;
    public class OnGridObjectChangedArgs : EventArgs
    {
        public int x;
        public int y;
    }

    private int width;
    private int height;
    private TGridObject[,] gridArray;

    public GenericGrid(int width, int height, Func<GenericGrid<TGridObject>, int, int, TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;

        gridArray = new TGridObject[width, height];

        // initialize
        for(int x = 0; x < width; x++) {
            for(int y = 0; y < height; y++) {
                gridArray[x, y] = createGridObject(this, x, y);
            }
        }
    }

    public void SetGridObject(int x, int y, TGridObject value) {
        if (!IsIndexValid(x, y)) return;

        gridArray[x, y] = value;
    }

    /// <summary>
    /// Attempts to retrieve the value at the provided index
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public TGridObject GetGridObject(int x, int y)
    {
        if (!IsIndexValid(x, y)) return default(TGridObject);

        return gridArray[x, y];
    }

    public void TriggerGridObjectChanged(int x, int y)
    {
        if (!IsIndexValid(x, y)) return;

        if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedArgs { x = x, y = y });
    }

    /// <summary>
    /// Validates an index, ensuring it does not go out of bounds of the array
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private bool IsIndexValid(int x, int y) => x < width && x >= 0 && y < height && y >= 0;
}