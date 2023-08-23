using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Core.Control;
using UnityEngine.UI;
using DD.Core.Combat;

[RequireComponent(typeof(RawImage))]
public class Crosshair : MonoBehaviour
{
    private RawImage crosshairImage;

    private void Awake()
    {
        crosshairImage = GetComponent<RawImage>();
    }

    private void OnEnable()
    {
        InputManager.Instance.OnAimDown += Show;
        InputManager.Instance.OnAimUp += Hide;
    }

    private void OnDisable()
    {
        if (!InputManager.Instance) return;

        InputManager.Instance.OnAimDown -= Show;
        InputManager.Instance.OnAimUp -= Hide;
    }

    private void Show()
    {
        if (EquipmentManager.Instance.ActiveWeapon == null) return;

        crosshairImage.enabled = true;
    }

    private void Hide()
    {
        crosshairImage.enabled = false;
    }
}
