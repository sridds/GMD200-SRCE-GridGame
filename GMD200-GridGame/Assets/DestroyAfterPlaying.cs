using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterPlaying : MonoBehaviour
{
    AudioSource source;

    private void Start() => source = GetComponent<AudioSource>();

    void Update()
    {
        if (!source.isPlaying) Destroy(gameObject);
    }
}