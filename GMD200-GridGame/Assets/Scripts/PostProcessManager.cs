using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessManager : MonoBehaviour
{
    private PostProcessProfile profile;

    private ColorGrading colorGrading;

    [Header("Color Grading Settings")]

    [SerializeField] private Color color;

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

        colorGrading.colorFilter.value = this.color;
    }

    public void SetParam(Color color) => this.color = color;
    public void SetParam(float brightness) => this.brightness = brightness;
}
