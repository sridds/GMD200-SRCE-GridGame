using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public AudioClip GetSoundFromKey(string key)
    {
        // loop through and search for sound
        foreach(GameSound sound in GameSounds) {
            if(key == sound.Key) {
                return sound.Clip;
            }
        }

        // return null if a clip was not found
        return null;
    }

    [System.Serializable]
    public struct GameSound
    {
        public string Key;
        public AudioClip Clip;
    }
}
