using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Canvas inventoryCanvas;

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryCanvas.enabled = !inventoryCanvas.enabled;
        }
    }
}
