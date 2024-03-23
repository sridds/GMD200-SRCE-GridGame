using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Transition : MonoBehaviour
{
    public void StartTransition(Image targetImage, float alphaTarget, float duration, float fadeWaitTime) 
        => StartCoroutine(LerpFadeIn(targetImage, alphaTarget, duration, fadeWaitTime));
    IEnumerator LerpFadeIn(Image targetImage, float alphaTarget, float duration, float fadeWaitTime)
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
            float currentAlpha = Mathf.Lerp(startAlpha, alphaTarget, timeElapsed);

            //Set current alpha
            targetImage.color = new Color(
                targetImage.color.r,
                targetImage.color.g,
                targetImage.color.b,
                currentAlpha
                );

            yield return null;
        }

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
    //TMPro Overload method
    public void StartTransition(TextMeshProUGUI targetImage, float alphaTarget, float duration, float fadeWaitTime)
        => StartCoroutine(LerpFadeIn(targetImage, alphaTarget, duration, fadeWaitTime));
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
    IEnumerator LerpFadeIn(TextMeshProUGUI targetImage, float alphaTarget, float duration, float fadeWaitTime)
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
            float currentAlpha = Mathf.Lerp(startAlpha, alphaTarget, timeElapsed);

            //Set current alpha
            targetImage.color = new Color(
                targetImage.color.r,
                targetImage.color.g,
                targetImage.color.b,
                currentAlpha
                );

            yield return null;
        }

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
}
