using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GrassWave : MonoBehaviour
{
    [SerializeField]
    private Transform sprite;

    [SerializeField]
    private string swaySoundKey = "tall_grass_sway";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player") return;

        sprite.DOKill();
        sprite.DOShakeRotation(0.3f, 30, 10, 90, true, ShakeRandomnessMode.Harmonic);

        AudioHandler.instance.ProcessAudioData(transform, swaySoundKey);
    }
}
