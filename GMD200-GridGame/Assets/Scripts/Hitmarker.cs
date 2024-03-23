using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Hitmarker : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro text;

    /// <summary>
    /// Intantiate a hitmarker at the provided position
    /// </summary>
    /// <param name="position"></param>
    /// <param name="value"></param>
    public static void CreateHitmarker(Vector3 position, int value)
    {
        // instantiate and set value of hit marker
        Hitmarker h = Instantiate(GameAssets.Instance.HitmarkerAsset, position, Quaternion.identity);
        h.SetValue(value);
    }

    /// <summary>
    /// Sets the text to the provided value
    /// </summary>
    /// <param name="value"></param>
    private void SetValue(int value) => text.SetText(value.ToString());

    private void Cleanup() => Destroy(gameObject);
}
