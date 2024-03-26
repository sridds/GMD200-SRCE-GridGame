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

    [SerializeField] private Color vignetteColor;

    [Tooltip("How long the vignette will hold before loading death scene")]
    [SerializeField] private float vignetteHoldTime = 1f;

    [Tooltip("The vignette intesity that will be set when the player dies")]
    [SerializeField] private float targetVingette = 0.7f;

    private void Start()
    {
        profile = GetComponent<PostProcessVolume>().profile;
    }
     
    private void Update()
    {
        if (profile == null) return;

        if (colorGrading == null) profile.TryGetSettings(out colorGrading);

        if (vignette == null) profile.TryGetSettings(out vignette);

        //Daytime visuals
        var timeElapsed = Time.time - startTime;
        var percentage = GameManager.Instance.currentDayTimer / GameManager.Instance.maxDayTimer;

        colorGrading.colorFilter.value = gradient.Evaluate(percentage);
        colorGrading.brightness.value = brightnessOvertime.Evaluate(percentage);

        vignette.intensity.value = this.vignetteIntensity;

        vignette.color.value = this.vignetteColor;

        //Transition to next day
        if (percentage >= 0.99f)
            GameManager.Instance.NextDay();
    }
    /// <summary>
    /// Displays the visuals when you die
    /// </summary>
    public void DeathVisuals() => StartCoroutine(PlayDeathVisual(vignetteHoldTime, targetVingette));
    IEnumerator PlayDeathVisual(float loadSceneWait, float targetVignette)
    {
        while (vignetteIntensity <= targetVignette)
        {
            vignetteIntensity += Time.deltaTime;
            yield return null;
        }
        vignetteIntensity = targetVignette;
        yield return new WaitForSeconds(loadSceneWait);
        SceneLoader.loadScene(2);
    }
}
