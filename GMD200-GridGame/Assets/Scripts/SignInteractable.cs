using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignInteractable : MonoBehaviour, Interactable
{
    [field: SerializeField]
    public string InteractText { get; private set; }

    [SerializeField]
    private DialogueSO data;

    public void Interact()
    {
        DialogueHandler.Instance.QueueDialogue(data);
    }
}
