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

    [Header("Day Night Cycle Settings")]

    public float duration = 5f;

    [Header("Color Grading Settings")]

    [SerializeField] private Gradient gradient;

    [SerializeField] private float brightness;

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

        if (vignette == null) profile.TryGetSettings(out vignette);

        colorGrading.brightness.value = this.brightness;

        vignette.intensity.value = this.vignetteIntensity;

        //Daytime visuals
        var percentage = GameManager.Instance.currentDayTimer / GameManager.Instance.maxDayTimer;

        colorGrading.colorFilter.value = gradient.Evaluate(percentage);

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

    /// <summary>
    /// Set game brightness
    /// </summary>
    /// <param name="brightness"></param>
    public void SetParam(float brightness) => this.brightness = brightness;
}
