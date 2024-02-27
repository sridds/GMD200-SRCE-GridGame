using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueDebugger : MonoBehaviour
{
    [SerializeField]
    private DialogueSO dialogue;

    // immediately queue up test dialogue
    void Start() => DialogueHandler.Instance.QueueDialogue(dialogue);
}
