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

    [Header("Day Night Cycle Settings")]

    public float duration = 5f;

    [Header("Color Grading Settings")]

    [SerializeField] private Gradient gradient;

    [SerializeField] private float brightness;


    private void Start()
    {
        profile = GetComponent<PostProcessVolume>().profile;
    }

    private void Update()
    {
        if (profile == null) return;

        if (colorGrading == null) profile.TryGetSettings(out colorGrading);

        colorGrading.brightness.value = this.brightness;

        var timeElapsed = Time.time - startTime;
        //Debug.Log(timeElapsed);
        var percentage = Mathf.Sin(timeElapsed / duration * Mathf.PI * 2) * 0.5f + 0.5f;
        percentage = Mathf.Clamp01(percentage);
        Debug.Log(percentage);

        if (percentage >= 1)
            GameManager.Instance.NextDay();


        colorGrading.colorFilter.value = gradient.Evaluate(percentage);
    }
    public void SetParam(float brightness) => this.brightness = brightness;
}
