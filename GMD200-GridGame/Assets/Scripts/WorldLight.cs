using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Light))]
public class WorldLight : MonoBehaviour
{
    public float duration = 5f;
    [SerializeField] private Gradient gradient;
    private Light dayNightLight;
    private float startTime;

    void Awake()
    {
        dayNightLight = GetComponent<Light>();
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        var timeElapsed = Time.time - startTime;
        var percentage = Mathf.Sin(timeElapsed / duration * Mathf.PI * 2) * 0.5f + 0.5f;
        percentage = Mathf.Clamp01(percentage);

        dayNightLight.color = gradient.Evaluate(percentage);
    }
}
