using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSource : MonoBehaviour
{
    [field: SerializeField] public bool IsPoweredOn { private set; get; }
    public event Action OnPoweredOn;
    public event Action OnPoweredOff;

    public void PowerOn()
    {
        IsPoweredOn = true;
        OnPoweredOn?.Invoke();
    }

    public void PowerOff()
    {
        IsPoweredOn = false;
        OnPoweredOff?.Invoke();
    }
}
