using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shelter : MonoBehaviour, Interactable
{
    [SerializeField] private string prompt = "Press [E] to sleep";
    public void Interact() => GameManager.Instance.NextDay();
}
