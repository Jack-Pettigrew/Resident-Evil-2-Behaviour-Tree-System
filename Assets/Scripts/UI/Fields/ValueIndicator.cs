using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DD.UI
{
    public abstract class ValueIndicator<T> : MonoBehaviour
    {
        protected TextMeshProUGUI textComponent;

        protected void Awake()
        {
            textComponent = GetComponent<TextMeshProUGUI>();
        }

        public abstract void UpdateIndicator(T value);
    }
}
