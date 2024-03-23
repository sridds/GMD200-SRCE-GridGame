using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeBookInteractable : MonoBehaviour, Interactable
{
    [field: SerializeField]
    public string InteractText { get; private set; }

    public void Interact()
    {
        FindObjectOfType<RecipeDisplay>().Open();
    }
}
