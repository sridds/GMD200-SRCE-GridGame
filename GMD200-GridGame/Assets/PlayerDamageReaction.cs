using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerDamageReaction : MonoBehaviour
{
    private Health health;

    [SerializeField]
    private string playerDamageSoundKey = "player_hurt";

    [SerializeField]
    private Image redScreenHurtEffect;

    void Start()
    {
        health = GetComponent<Health>();
        health.OnHealthDecrease += React;
    }

    private void React(int newHP)
    {
        CameraShake.instance.Shake(0.3f, 0.2f);
        AudioHandler.instance.ProcessAudioData(transform, playerDamageSoundKey);
        redScreenHurtEffect.color = new Color(redScreenHurtEffect.color.r, redScreenHurtEffect.color.g, redScreenHurtEffect.color.b, 0.4f);
        redScreenHurtEffect.DOFade(0, 0.5f);
    }
}
