using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DD.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class SliderValueIndicator : ValueIndicator<float>
    {
        public override void UpdateIndicator(float value)
        {
            textComponent.text = value.ToString("F2");
        }
    }
}
