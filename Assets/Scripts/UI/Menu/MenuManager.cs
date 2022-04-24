using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject inventoryUI;

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryUI.SetActive(!inventoryUI.activeInHierarchy);
        }
    }
}
