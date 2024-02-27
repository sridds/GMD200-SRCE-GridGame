using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "DialogueAsset", menuName = "Dialogue/Dialogue", order = 1)]
public class DialogueSO : ScriptableObject
{
    public DialogueData[] dialogue;
}

[System.Serializable]
public struct DialogueData
{
    [TextArea]
    public string Line;

    // character settings
    public bool HasCharacter;
    [ShowIf(nameof(HasCharacter)), AllowNesting]
    public CharacterSO Character;
}
