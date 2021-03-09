using System.Collections;
using UnityEngine;
public class Fader : MonoBehaviour
{
    CanvasGroup canvasGroup;
    Coroutine currentActiveCoroutine = null;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void FadeOutImmidiately()
    {
        canvasGroup.alpha = 1;
    }

    public Coroutine FadeOut(float time)
    {
        return Fade(1, time);
    }
    public Coroutine FadeIn(float time)
    {
        return Fade(0, time);
    }

    Coroutine Fade(float target, float time)
    {
        if (currentActiveCoroutine != null)
        {
            StopCoroutine(currentActiveCoroutine);
        }
        currentActiveCoroutine = StartCoroutine(FadeRoutine(target, time));
        return currentActiveCoroutine;
    }

    IEnumerator FadeRoutine(float target, float time)
    {
        while (!Mathf.Approximately(canvasGroup.alpha, target))
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / time);
            yield return null;
        }
    }
}