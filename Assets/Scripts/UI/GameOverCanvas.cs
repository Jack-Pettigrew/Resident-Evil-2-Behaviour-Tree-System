using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DD.Core.Control;

namespace DD.UI
{
    public class GameOverCanvas : MonoBehaviour
    {
        [field: Header("Fade Transition")]
        [field: SerializeField, ReadOnly] public bool isTransitioning { private set; get; } = false;
        public float fadeTime = 1.0f;
        [SerializeField] protected CanvasGroup fadingCavasGroup;

        // Button Logic
        [SerializeField] private Button retryButton;

        // COROUTINE
        protected Coroutine fadeCoroutine;

        public void ToggleUI(bool toggle)
        {
            if(toggle)
            {
                StartCoroutine(FadeCorountine(0, 1, fadeTime, () => fadingCavasGroup.blocksRaycasts = true));
                InputManager.Instance.CursorToggle();
            }
            else
            {
                StartCoroutine(FadeCorountine(1, 0, fadeTime, () => fadingCavasGroup.blocksRaycasts = false));
                InputManager.Instance.CursorToggle();
            }
        }

        protected virtual IEnumerator FadeCorountine(float startValue, float endValue, float fadeLength, Action onComplete = null)
        {
            if(isTransitioning)
            {
                StopCoroutine(fadeCoroutine);
                startValue = fadingCavasGroup.alpha;
            }

            isTransitioning = true;
            
            startValue = Mathf.Clamp01(startValue);
            endValue = Mathf.Clamp01(endValue);

            fadingCavasGroup.alpha = startValue;

            while(fadingCavasGroup.alpha != endValue)
            {
                // Fade Direction
                if(endValue > startValue)
                {
                    fadingCavasGroup.alpha += (1 / fadeTime) * Time.deltaTime;
                }
                else
                {
                    fadingCavasGroup.alpha -= (1 / fadeTime) * Time.deltaTime;
                }
                
                yield return null;
            }

            isTransitioning = false;
            onComplete?.Invoke();
        }        
    }
}
