using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// This was inspired by CodeMonkey's tutorial
/// </summary>
public class GameAssets : MonoBehaviour
{
    private static GameAssets instance;

    public static GameAssets Instance {
        get {
            if (instance == null) instance = Instantiate(Resources.Load<GameAssets>("GameAssets"));
            return instance;
        }
    }

    public GameSound[] GameSounds;
    public Hitmarker HitmarkerAsset;
    public ItemDrop ItemDropPrefab;

    public GameSound GetSoundFromKey(string key)
    {
        // loop through and search for sound
        foreach(GameSound sound in GameSounds) {
            if(key == sound.Key) {
                return sound;
            }
        }

        Debug.Log($"Sound of key: {key} does not exist!");
        // return null if a clip was not found
        return null;
    }
}

[System.Serializable]
public class GameSound
{
    public string Key;
    public AudioClip[] Clips;
    public float DefaultVolume;

    [Header("Pitch")]
    public bool RandomizePitch;
    public float Pitch;

    [AllowNesting]
    [ShowIf(nameof(RandomizePitch))]
    public float MaxPitch;
}
