using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessManager : MonoBehaviour
{
    private PostProcessProfile profile;
    private ColorGrading colorGrading;

    private float startTime;

    [Header("Color Grading Settings")]

    [SerializeField] private Gradient gradient;
    [SerializeField] private AnimationCurve brightnessOvertime;


    private void Start()
    {
        profile = GetComponent<PostProcessVolume>().profile;
    }

    private void Update()
    {
        if (profile == null) return;

        if (colorGrading == null) profile.TryGetSettings(out colorGrading);

        var timeElapsed = Time.time - startTime;
        var percentage = GameManager.Instance.currentDayTimer / GameManager.Instance.maxDayTimer;
       
        colorGrading.colorFilter.value = gradient.Evaluate(percentage);
        colorGrading.brightness.value = brightnessOvertime.Evaluate(percentage);
    }
}
