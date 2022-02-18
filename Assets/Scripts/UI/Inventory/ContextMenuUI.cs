using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContextMenuUI : MonoBehaviour
{
    [SerializeField] private Animation ani;
    
    public void ShowMenu(Transform targetTransform)
    {
        gameObject.transform.position = targetTransform.position;
        ani.Play("UIFadeIn");
    }

    public void HideMenu()
    {
        ani.Play("UIFadeOut");
    }
}
