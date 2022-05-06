using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.UI
{
    public abstract class GameplayUI : MonoBehaviour, IFadable
    {
        [Header("Fade Transition")]
        public bool isTransitioning = false;
        public float fadeTime = 1.0f;
        [SerializeField] protected CanvasGroup fadingCavasGroup;

        // COROUTINE
        protected Coroutine fadeCoroutine;
        
        public abstract void UpdateUI();
        
        public bool IsVisible()
        {
            return fadingCavasGroup.alpha > 0;
        }
        
        public void StartFadeTransition(float startValue, float endValue, float fadeLength)
        {
            fadeCoroutine = StartCoroutine(FadeCorountine(startValue, endValue, fadeLength));
        }

        protected virtual IEnumerator FadeCorountine(float startValue, float endValue, float fadeLength)
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
        }
    }
}
