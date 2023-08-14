using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flicker : MonoBehaviour
{
    [SerializeField] private MonoBehaviour componentToFlicker;
    [SerializeField, Min(0)] private float flickerWaitSecs = 1.0f;
    private Coroutine flickerCorountine;
    
    private void OnEnable() {
        if (componentToFlicker != null)
        {
            flickerCorountine = StartCoroutine(UpdateFlicker());
        }
    }


    private void OnDisable() {
        if (flickerCorountine != null)
        {
            StopCoroutine(flickerCorountine);
        }
    }
    
    private IEnumerator UpdateFlicker()
    {
        while (true)
        {
            componentToFlicker.enabled = !componentToFlicker.enabled;
            
            yield return new WaitForSeconds(flickerWaitSecs);
        }
    }
}
