using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { private set; get; }

    [Header("Transition Settings")]
    [SerializeField] private CanvasGroup transitionCanvasGroup;
    [SerializeField, Min(0)] private float fadeDelay = 0.0f;
    [SerializeField] private float transitionFadeTime = 1.0f;

    public bool IsLoading { private set; get; } = false;

    private int sceneBuildIndexTransitioningTo = -1;
    private int activeSceneBuildIndex = -1;
    private AsyncOperation asyncOperation;

    public UnityEvent OnLoadStarted;
    public UnityEvent OnLoadFinished;

    private void Awake()
    {
        if (Instance != this)
        {
            Instance = this;
        }
    }

    [ContextMenu("Test Load")]
    public void TestLoad()
    {
        LoadSceneAsync(1);
    }

    public void LoadSceneAsync(int sceneBuildIndex)
    {
        sceneBuildIndexTransitioningTo = sceneBuildIndex;
        StartCoroutine(TransitionCoroutine());
    }

    private IEnumerator TransitionCoroutine()
    {
        IsLoading = true;

        yield return FadeCorountine(0, 1, transitionFadeTime);

        OnLoadStarted?.Invoke();

        if (activeSceneBuildIndex > -1)
        {
            asyncOperation = SceneManager.UnloadSceneAsync(activeSceneBuildIndex);
            yield return new WaitUntil(() => asyncOperation.progress >= 1.0f);
            activeSceneBuildIndex = -1;
        }

        asyncOperation = SceneManager.LoadSceneAsync(sceneBuildIndexTransitioningTo, LoadSceneMode.Additive);
        asyncOperation.allowSceneActivation = false;

        yield return new WaitUntil(() => asyncOperation.progress >= 0.9f);

        asyncOperation.allowSceneActivation = true;
        yield return new WaitUntil(() => asyncOperation.isDone);

        OnLoadFinished?.Invoke();

        yield return new WaitForSeconds(fadeDelay);

        yield return FadeCorountine(1, 0, transitionFadeTime);

        IsLoading = true;
        activeSceneBuildIndex = sceneBuildIndexTransitioningTo;
        sceneBuildIndexTransitioningTo = -1;
    }

    private IEnumerator FadeCorountine(float startValue, float endValue, float fadeLength)
    {
        startValue = Mathf.Clamp01(startValue);
        endValue = Mathf.Clamp01(endValue);

        transitionCanvasGroup.alpha = startValue;

        while (transitionCanvasGroup.alpha != endValue)
        {
            // Fade Direction
            if (endValue > startValue)
            {
                transitionCanvasGroup.alpha += (1 / transitionFadeTime) * Time.deltaTime;
            }
            else
            {
                transitionCanvasGroup.alpha -= (1 / transitionFadeTime) * Time.deltaTime;
            }

            yield return null;
        }
    }
}
