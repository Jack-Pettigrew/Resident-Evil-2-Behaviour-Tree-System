using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CommunityLabel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;

    public void SetLabel(string labelText) => label.text = labelText;
}
