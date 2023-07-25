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

    private bool hasLink = false;
    
    public void SetTitle(string text)
    {
        title.text = text;
    }

    public void SetAuthor(string text)
    {
        author.text = text;
    }

    public void SetLink(string text)
    {
        if(text == string.Empty)
        {
            hasLink = false;
            if (TryGetComponent<ClickableUI>(out ClickableUI clickableUI))
            {
                clickableUI.OnClick.RemoveAllListeners();
            }
        }        
        else if (TryGetComponent<ClickableUI>(out ClickableUI clickableUI))
        {
            clickableUI.OnClick.AddListener(() => URLOpener.OpenURL(text));
            hasLink = true;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        backgroundPanel.color = hoverColour;

        if(hasLink)
        {
            title.fontStyle = FontStyles.Underline;
            author.fontStyle = FontStyles.Underline;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        backgroundPanel.color = defaultColour;

        if(hasLink)
        {
            title.fontStyle = FontStyles.Normal;
            author.fontStyle = FontStyles.Normal;
        }
    }
}
