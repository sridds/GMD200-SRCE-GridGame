using UnityEngine.UI;
using UnityEngine;

public class RecipeDisplay : MonoBehaviour
{
    [SerializeField]
    private Listing _listOption;

    [SerializeField]
    private RectTransform _content;

    [SerializeField]
    private Image[] _slots;

    private CraftingGrid craftingGrid;

    // Start is called before the first frame update
    void Start()
    {
        craftingGrid = FindObjectOfType<CraftingGrid>();

        // add current recipes
        foreach(RecipeSO recipe in craftingGrid.Recipes)
        {
            AddRecipe(recipe);
        }

        // subscribe so other recipes can be added
        craftingGrid.OnRecipeAdded += AddRecipe;
    }

    void AddRecipe(RecipeSO recipe)
    {
        Listing listing = Instantiate(_listOption, _content);
        listing.Initialize(recipe);
    }

    public void SelectRecipe(RecipeSO recipe)
    {
        for(int i = 0; i < _slots.Length; i++)
        {
            if (recipe.materials[i] != null) {
                _slots[i].enabled = true;
                _slots[i].sprite = recipe.materials[i].ItemSprite;
            }
            else {
                _slots[i].enabled = false;
            }
        }
    }
}
