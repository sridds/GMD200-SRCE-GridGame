using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingGrid : MonoBehaviour
{
    [SerializeField]
    private ItemGrid myItemGrid;

    [SerializeField]
    private ItemGrid outputSlot;

    [SerializeField]
    private SlotUI outputSlotUI;

    [SerializeField]
    private List<RecipeSO> recipies = new();

    public List<RecipeSO> Recipes { get { return recipies; } }
    public delegate void RecipeAdded(RecipeSO recipe);
    public RecipeAdded OnRecipeAdded;

    RecipeSO activeRecipe = null;

    private void Start()
    {
        myItemGrid.Slots.OnGridObjectChanged += GridModified;
        outputSlotUI.OnSlotTaken += OutputChanged;
    }

    /// <summary>
    /// Adds a recipe to the list
    /// </summary>
    /// <param name="recipe"></param>
    public void AddRecipe(RecipeSO recipe)
    {
        if (recipies.Contains(recipe)) return;

        recipies.Add(recipe);
        OnRecipeAdded?.Invoke(recipe);
    }

    private void OutputChanged()
    {
        Slot output = outputSlot.Slots.GetGridObject(0, 0);

        DecrementMatchingItems(activeRecipe);
    }

    private void GridModified(object sender, GenericGrid<Slot>.OnGridObjectChangedArgs e)
    {
        Slot output = outputSlot.Slots.GetGridObject(0, 0);
        output.ResetSlot();

        // check for an output item
        foreach (RecipeSO recipe in recipies)
        {
            if (ValidateRecipe(recipe)) {
                output.SetItem(recipe.OutputItem, recipe.OutputAmount);
                activeRecipe = recipe;

                break;
            }
        }
    }

    private bool ValidateRecipe(RecipeSO recipe)
    {
        bool completeRecipe = true;

        for (int x = 0; x < myItemGrid.Dimensions.x; x++)
        {
            for (int y = 0; y < myItemGrid.Dimensions.y; y++)
            {
                if (recipe.materials[y * myItemGrid.Dimensions.x + x] != null) {
                    Slot slot = myItemGrid.Slots.GetGridObject(x, y);

                    if (slot.Item == null || slot.Item.ItemName != recipe.materials[y * myItemGrid.Dimensions.x + x].ItemName) {
                        completeRecipe = false;
                    }
                }
                else if(myItemGrid.Slots.GetGridObject(x, y).Item != null) {
                    completeRecipe = false;
                }
            }
        }
        return completeRecipe;
    }

    private void DecrementMatchingItems(RecipeSO recipe)
    {
        for (int x = 0; x < myItemGrid.Dimensions.x; x++)
        {
            for (int y = 0; y < myItemGrid.Dimensions.y; y++)
            {
                if (recipe.materials[y * myItemGrid.Dimensions.x + x] != null)
                {
                    Slot slot = myItemGrid.Slots.GetGridObject(x, y);

                    if (slot.Item != null && slot.Item.ItemName == recipe.materials[y * myItemGrid.Dimensions.x + x].ItemName)
                    {
                        slot.RemoveFromStack();
                    }
                }
            }
        }
    }
}
