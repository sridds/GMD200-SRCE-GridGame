using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessManager : MonoBehaviour
{
    private PostProcessProfile profile;
    private ColorGrading colorGrading;

    private Vignette vignette;

    private float startTime;

    [Header("Color Grading Settings")]

    [SerializeField] private Gradient gradient;
    [SerializeField] private AnimationCurve brightnessOvertime;

    [Header("Vignette Settings")]

    [SerializeField] private float vignetteIntensity;

    private void Start()
    {
        profile = GetComponent<PostProcessVolume>().profile;
    }

    private void Update()
    {
        if (profile == null) return;

        if (colorGrading == null) profile.TryGetSettings(out colorGrading);


        //Daytime visuals
        var timeElapsed = Time.time - startTime;
        var percentage = GameManager.Instance.currentDayTimer / GameManager.Instance.maxDayTimer;

        colorGrading.colorFilter.value = gradient.Evaluate(percentage);
        colorGrading.brightness.value = brightnessOvertime.Evaluate(percentage);

        vignette.intensity.value = this.vignetteIntensity;

        //Transition to next day
        if (percentage >= 0.99f)
            GameManager.Instance.NextDay();
    }

    public void DeathVisuals()
    {
        vignetteIntensity += Time.deltaTime;
        if (vignetteIntensity >= 0.8f)
            vignetteIntensity = 0.8f;

        SceneLoader.loadScene(2);
    }
}
