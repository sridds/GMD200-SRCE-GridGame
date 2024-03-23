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
        Destroy(gameObject);
    }
}
