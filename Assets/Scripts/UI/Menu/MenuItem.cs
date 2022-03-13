using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DD.UI
{
    public abstract class MenuItem : MonoBehaviour, IPointerClickHandler
    {        
        public void OnPointerClick(PointerEventData eventData)
        {
            Select();
        }

        public abstract void Select();
    }
}
