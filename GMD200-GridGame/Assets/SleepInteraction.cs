using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepInteraction : MonoBehaviour, Interactable
{
    [field: SerializeField]
    public string InteractText { get; private set; }

    public void Interact()
    {
        GameManager.Instance.NextDay();
    }
}
