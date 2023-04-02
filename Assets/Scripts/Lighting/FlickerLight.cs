using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class FlickerLight : MonoBehaviour
{
    // FLICKER COMPONENTS
    private Light lightComponent;

    // FLICKER SETTINGS
    [SerializeField] private string flickerPattern = "mmamammmmammamamaaamammma";
    [SerializeField] private float flickerDelay = 1.0f;

    // FLICKER STATE
    private float flickerTimer = 0.0f;
    private static Dictionary<char, bool> patternDictionary = null;

    private void Awake()
    {
        lightComponent = GetComponent<Light>();

        if (patternDictionary == null)
        {
            patternDictionary = new Dictionary<char, bool>();

            patternDictionary.Add('m', true);
            patternDictionary.Add('a', false);
        }

        foreach (char character in flickerPattern)
        {
            if (!patternDictionary.ContainsKey(character))
            {
                Debug.LogWarning("Flicker Pattern contains an unknown character. Emptying pattern - the light won't flicker", this);
                flickerPattern = string.Empty;
            }
        }
    }

    private void Start()
    {
        if (string.Empty != flickerPattern)
        {
            StartCoroutine(Flicker());
        }
    }

    private IEnumerator Flicker()
    {
        while (true)
        {
            foreach (char character in flickerPattern)
            {
                flickerTimer += Time.deltaTime;

                if (flickerTimer >= flickerDelay)
                {
                    flickerTimer = 0.0f;
                    lightComponent.enabled = patternDictionary[character];
                }

                yield return null;
            }
        }
    }
}
