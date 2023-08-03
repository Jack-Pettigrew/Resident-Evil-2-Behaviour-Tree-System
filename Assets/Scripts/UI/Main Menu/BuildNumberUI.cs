using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildNumberUI : MonoBehaviour
{
    private void Awake() {
        GetComponent<TextMeshProUGUI>().text += Application.version;
    }
}
