using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shelter : MonoBehaviour, Interactable
{
    [field: SerializeField]
    public string InteractText { get; private set; } 

    public void Interact() => GameManager.Instance.NextDay();
}
