using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Credit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI author;
    [SerializeField] private Image backgroundPanel;
    [SerializeField] private Color defaultColour;
    [SerializeField] private Color hoverColour;
    
    public void SetTitle(string text)
    {
        title.text = text;
    }

    public void SetAuthor(string text)
    {
        author.text = text;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        backgroundPanel.color = hoverColour;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        backgroundPanel.color = defaultColour;
    }
}
