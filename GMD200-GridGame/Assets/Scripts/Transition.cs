using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Transition : MonoBehaviour
{
    [Header("Transition Settings")]
    [Tooltip("The target alpha")]
    [Range(0, 1)]
    [SerializeField] private float targetTransitionAlpha = 1f;

    [Tooltip("The time it takes to fade into black")]
    [SerializeField] private float transitionTime = 1f;

    [Tooltip("The time until it fades out of black")]
    [SerializeField] private float fadeOutWaitTime = 1f;

    //Image
    /// <summary>
    /// Fades an image in and out
    /// </summary>
    /// <param name="targetImage"></param>
    public void StartTransition(Image targetImage) 
        => StartCoroutine(LerpFadeIn(targetImage, targetTransitionAlpha, transitionTime, fadeOutWaitTime));
    IEnumerator LerpFadeIn(Image targetImage, float alphaTarget, float duration, float fadeWaitTime)
    {
        //Set alpha to target alpha
        targetImage.color = new Color(
                targetImage.color.r,
                targetImage.color.g,
                targetImage.color.b,
                alphaTarget
                );

        yield return new WaitForSeconds(fadeWaitTime);
        StartCoroutine(LerpFadeOut(targetImage, duration));
    }
    IEnumerator LerpFadeOut(Image targetImage, float duration)
    {
        //Local Variables
        float startAlpha = targetImage.color.a;
        float time = 0;

        while (time <= duration)
        {
            time += Time.deltaTime;

            //Find current time elapsed
            float timeElapsed = time / duration;

            //Calculate current alpha
            float currentAlpha = Mathf.Lerp(startAlpha, 0, timeElapsed);

            //Set current alpha
            targetImage.color = new Color(
                targetImage.color.r,
                targetImage.color.g,
                targetImage.color.b,
                currentAlpha
                );

            yield return null;
        }
        targetImage.color = new Color(
                targetImage.color.r,
                targetImage.color.g,
                targetImage.color.b,
                0
                );
    }

    //Text Overload method
    /// <summary>
    /// Fades a text in and out
    /// </summary>
    /// <param name="targetImage"></param>
    public void StartTransition(TextMeshProUGUI targetImage)
        => StartCoroutine(LerpFadeIn(targetImage, targetTransitionAlpha, transitionTime, fadeOutWaitTime));
    IEnumerator LerpFadeIn(TextMeshProUGUI targetImage, float alphaTarget, float duration, float fadeWaitTime)
    {
        //Set alpha to target alpha
        targetImage.color = new Color(
                targetImage.color.r,
                targetImage.color.g,
                targetImage.color.b,
                alphaTarget
                );

        yield return new WaitForSeconds(fadeWaitTime);
        StartCoroutine(LerpFadeOut(targetImage, duration));
    }
    IEnumerator LerpFadeOut(TextMeshProUGUI targetImage, float duration)
    {
        //Local Variables
        float startAlpha = targetImage.color.a;
        float time = 0;

        while (time <= duration)
        {
            time += Time.deltaTime;

            //Find current time elapsed
            float timeElapsed = time / duration;

            //Calculate current alpha
            float currentAlpha = Mathf.Lerp(startAlpha, 0, timeElapsed);

            //Set current alpha
            targetImage.color = new Color(
                targetImage.color.r,
                targetImage.color.g,
                targetImage.color.b,
                currentAlpha
                );

            yield return null;
        }
        targetImage.color = new Color(
                targetImage.color.r,
                targetImage.color.g,
                targetImage.color.b,
                0
                );
    }
}
