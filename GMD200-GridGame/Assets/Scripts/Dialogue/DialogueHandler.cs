using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueHandler : MonoBehaviour
{
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

    private Queue<string> dialogueQueue = new Queue<string>();
    private Coroutine activeDialogueCoroutine;

    private string currentLine;
    private bool specialCharacter = false;
    private bool continueFlag = false;

    private void Start()
    {
        // test dialogue
        QueueDialogue("HELP HELP I love my\\! ROCKS !!!! "); // default pause time
        QueueDialogue("I THROW \\@ROCKS AND HIT PEOPLE WITH ROCKS ITS AWESOME!! "); // medium pause time
        QueueDialogue("would you like to throw rocks at\\# people with m,e "); // long pause
    }

    /// <summary>
    /// Queues up an array of dialogue lines
    /// </summary>
    /// <param name="dialogue"></param>
    public void QueueDialogue(string[] dialogue)
    {
        // queue up each dialogue and immediately continue
        foreach(string s in dialogue) dialogueQueue.Enqueue(s);
        Continue();
    }

    /// <summary>
    /// Queues a single line of dialogue
    /// </summary>
    /// <param name="dialogue"></param>
    public void QueueDialogue(string dialogue)
    {
        // queue up the dialogue and immediately start
        dialogueQueue.Enqueue(dialogue);
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

        if (CanCloseDialogueBox()) _dialogueBox.SetActive(false);

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
        _dialogueUI.text = "";
        for(int i = 0; i < currentLine.Length; i++)
        {
            // skip special characters
            if (currentLine[i] == '\\')
            {
                i++; // increment to skip over the special character
                continue;
            }

            // add to current line
            _dialogueUI.text += currentLine[i];
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
    /// <param name="line"></param>
    /// <returns></returns>
    private IEnumerator HandleDialogue(string line)
    {
        _dialogueBox.SetActive(true);

        // clear text
        _dialogueUI.text = "";

        // iterate through each char and add it to the text.
        for (int i = 0; i < line.Length; i++)
        {
            // mark special characters as enabled
            if (line[i] == '\\') {
                specialCharacter = true;
                continue;
            }

            // switch on the special character and perform the necessary logic
            if (specialCharacter)
            {
                switch (line[i]){
                    case '!':
                        yield return new WaitForSecondsRealtime(_defaultPauseTime);
                        break;
                    case '@':
                        yield return new WaitForSecondsRealtime(_mediumPauseTime);
                        break;
                    case '#':
                        yield return new WaitForSecondsRealtime(_longPauseTime);
                        break;
                }

                specialCharacter = false;
                continue;
            }

            // add to the text
            _dialogueUI.text += line[i];
            yield return new WaitForSeconds(_textSpeed);
        }

        // set the active dialogue coroutine to null
        activeDialogueCoroutine = null;
    }
}
