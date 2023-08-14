using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressToContinue : MonoBehaviour
{
    [SerializeField] private bool anyKey;
    [SerializeField, Tooltip("Only considered if anyKey is unchecked.")] private KeyCode keyCode;
    
    public UnityEvent OnContinue;
    
    // Update is called once per frame
    void Update()
    {
        if(anyKey && Input.anyKeyDown)
        {
            OnContinue?.Invoke();
        }
        else if(Input.GetKeyDown(keyCode))
        {
            OnContinue?.Invoke();
        }
    }
}
