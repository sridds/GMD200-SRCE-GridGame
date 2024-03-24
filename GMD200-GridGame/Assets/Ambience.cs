using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ambience : MonoBehaviour
{
    [SerializeField]
    private AudioSource daySource;

    [SerializeField]
    private AudioSource nightSource;

    [SerializeField]
    private float nightTimeThreshold = 5;

    private float initNightVol;
    private float initDayVol;

    AudioSource tempSource;
    bool switched;

    private void Start()
    {
        initDayVol = daySource.volume;
        initNightVol = nightSource.volume;

        ResetSources();
        GameManager.Instance.OnDayUpdate += ResetSources;
    }

    public void Update()
    {
        if(GameManager.Instance.currentDayTimer > nightTimeThreshold && !switched)
        {
            daySource.DOFade(0, 5.0f);
            nightSource.DOFade(initNightVol, 5.0f);
            switched = true;
        }
    }

    private void ResetSources()
    {
        daySource.DOFade(initDayVol, 1.0f);
        nightSource.DOFade(0, 1.0f);

        switched = false;
    }
}
