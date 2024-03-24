using UnityEngine.UI;
using UnityEngine;

public class RecipeDisplay : MonoBehaviour
{
    [SerializeField]
    private Listing _listOption;

    [SerializeField]
    private GameObject _recipeBookHolder;

    [SerializeField]
    private RectTransform _content;

    [SerializeField]
    private Image[] _slots;

    private CraftingGrid craftingGrid;
    private bool isOpen;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Close();
    }

    public void Open()
    {
        // some other UI is open
        if (!isOpen && GameManager.Instance.currentGameState == GameState.UI) return;
        if (GameManager.Instance.currentGameState == GameState.Paused) return;

        isOpen = true;

        GameManager.Instance.currentGameState = GameState.UI;
        _recipeBookHolder.gameObject.SetActive(true);
    }

    public void Close()
    {
        if (!isOpen) return;

        GameManager.Instance.currentGameState = GameState.Playing;
        _recipeBookHolder.gameObject.SetActive(false);
    }
}
