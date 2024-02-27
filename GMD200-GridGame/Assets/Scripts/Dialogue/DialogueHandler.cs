using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueHandler : MonoBehaviour
{
    public static DialogueHandler Instance { get; private set; }

    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI _dialogueUI;
    [SerializeField]
    private GameObject _dialogueBox;

    [Header("Modifiers")]
    [SerializeField]
    private float _textSpeed = 0.05f;

    [Header("Special Characters")]
    [SerializeField]
    private float _defaultPauseTime = 0.3f;
    [SerializeField]
    private float _mediumPauseTime = 0.5f;
    [SerializeField]
    private float _longPauseTime = 0.8f;

    private Queue<DialogueData> dialogueQueue = new Queue<DialogueData>();
    private Coroutine activeDialogueCoroutine;

    private DialogueData currentLine;
    private bool specialCharacter = false;
    private bool continueFlag = false;

    private void Awake()
    {
        // Setup instance
        if(Instance == null) Instance = this;

        CloseDialogueBox();
    }

    /// <summary>
    /// Queues up dialogue data from dialogue scriptable object
    /// </summary>
    /// <param name="dialogue"></param>
    public void QueueDialogue(DialogueSO data)
    {
        foreach(DialogueData d in data.dialogue) dialogueQueue.Enqueue(d);
        Continue();
    }

    /// <summary>
    /// Queues up a single dialogue data.
    /// </summary>
    /// <param name="data"></param>
    public void QueueDialogue(DialogueData data)
    {
        dialogueQueue.Enqueue(data);
        Continue();
    }

    private void Update()
    {
        // get input to continue / skip dialogue
        if (Input.GetKeyDown(KeyCode.Z)) Continue();
        if (Input.GetKeyDown(KeyCode.X)) Skip();

        // handle dialogue by checking if dialogue can be dequeued and started
        if (CanHandleDialogue()) {
            currentLine = dialogueQueue.Dequeue();
            activeDialogueCoroutine = StartCoroutine(HandleDialogue(currentLine));
        }

        if (CanCloseDialogueBox()) CloseDialogueBox();

        // reset flags
        continueFlag = false;
    }

    /// <summary>
    /// Indicates whether or not the dialogue has finished and can be closed
    /// </summary>
    /// <returns></returns>
    private bool CanCloseDialogueBox()
    {
        if (!continueFlag || activeDialogueCoroutine != null || dialogueQueue.Count > 0) return false;

        return true;
    }

    private void CloseDialogueBox()
    {
        _dialogueUI.text = "";
        _dialogueBox.SetActive(false);
    }

    /// <summary>
    /// Sets the continue flag for the next line to be queued
    /// </summary>
    private void Continue()
    {
        if (activeDialogueCoroutine != null) return;

        continueFlag = true;
    }

    /// <summary>
    /// Speeds up the current line to the end of the line
    /// </summary>
    private void Skip()
    {
        // The dialouge must be currently running
        if (activeDialogueCoroutine == null) return;

        // stop the current dialogue coroutine
        StopCoroutine(activeDialogueCoroutine);
        activeDialogueCoroutine = null;

        // reset dialogue ui and 
        _dialogueUI.text = currentLine.HasCharacter ? $"{currentLine.Character.name.ToUpper()}: " : "";

        for (int i = 0; i < currentLine.Line.Length; i++)
        {
            // skip special characters
            if (currentLine.Line[i] == '\\')
            {
                i++; // increment to skip over the special character
                continue;
            }

            // add to current line
            _dialogueUI.text += currentLine.Line[i];
        }
    }

    private bool CanHandleDialogue()
    {
        // guard clauses
        if (dialogueQueue.Count == 0) return false; // must be a populated queue
        if (!continueFlag) return false;

        return true;
    }

    /// <summary>
    /// Handles a single line of dialogue
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private IEnumerator HandleDialogue(DialogueData data)
    {
        _dialogueBox.SetActive(true);

        // clear text
        _dialogueUI.text = currentLine.HasCharacter ? $"{currentLine.Character.name.ToUpper()}: " : "";
        yield return null;

        // iterate through each char and add it to the text.
        for (int i = 0; i < data.Line.Length; i++)
        {
            // mark special characters as enabled
            if (data.Line[i] == '\\') {
                specialCharacter = true;
                continue;
            }

            // switch on the special character and perform the necessary logic
            if (specialCharacter)
            {
                switch (data.Line[i]){
                    case '!':
                        yield return new WaitForSeconds(_defaultPauseTime);
                        break;
                    case '@':
                        yield return new WaitForSeconds(_mediumPauseTime);
                        break;
                    case '#':
                        yield return new WaitForSeconds(_longPauseTime);
                        break;
                }

                specialCharacter = false;
                continue;
            }

            // add to the text
            _dialogueUI.text += data.Line[i];
            yield return new WaitForSeconds(_textSpeed);
        }

        // set the active dialogue coroutine to null
        activeDialogueCoroutine = null;
    }
}
