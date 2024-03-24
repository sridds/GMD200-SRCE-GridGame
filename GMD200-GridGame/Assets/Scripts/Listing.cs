using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Listing : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private Image icon;

    RecipeSO myRecipe;
    RecipeDisplay display;

    public void Initialize(RecipeSO myRecipe)
    {
        this.myRecipe = myRecipe;

        // setup text
        string countText = myRecipe.OutputAmount > 1 ? $"{myRecipe.OutputAmount}x " : "";
        text.text = $"{countText}{myRecipe.OutputItem.ItemName}";

        // setup icon
        icon.sprite = myRecipe.OutputItem.ItemSprite;

        display = FindObjectOfType<RecipeDisplay>();
    }

    public void RecipeClicked() => display.SelectRecipe(myRecipe);
}
