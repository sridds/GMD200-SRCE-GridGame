using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldLight : MonoBehaviour
{
    private PostProcessManager postManager;

    public float duration = 5f;
    [SerializeField] private Gradient gradient;
    //private Light dayNightLight;
    private float startTime;

    void Awake()
    {
        postManager = GetComponent<PostProcessManager>();

        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        var timeElapsed = Time.time - startTime;
        var percentage = Mathf.Sin(timeElapsed / duration * Mathf.PI * 2) * 0.5f + 0.5f;
        percentage = Mathf.Clamp01(percentage);

        //When midnight
        //Enable night UI "Day ___"
        //Regenerate map
        //Disable UI

        //dayNightLight.color = gradient.Evaluate(percentage);
    }
}
