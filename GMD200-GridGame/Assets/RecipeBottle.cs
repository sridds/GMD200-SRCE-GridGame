using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeBottle : MonoBehaviour, Interactable
{
    [field: SerializeField]
    public string InteractText { get; private set; }

    [SerializeField]
    private RecipeSO myRecipe;

    public void Interact()
    {
        FindObjectOfType<CraftingGrid>().AddRecipe(myRecipe);
        FindObjectOfType<TooltipManager>().CreateTooltip(new TooltipData { headerText = "New Recipe!", icon = myRecipe.OutputItem.ItemSprite, subheaderText = myRecipe.OutputItem.ItemName });
        Destroy(gameObject);
    }
}
