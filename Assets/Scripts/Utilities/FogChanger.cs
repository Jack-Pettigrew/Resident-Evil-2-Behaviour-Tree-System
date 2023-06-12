using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(LocalVolumetricFog))]
public class FogChanger : MonoBehaviour
{
    private LocalVolumetricFog localVolumetricFog;
    
    private void Awake() {
        localVolumetricFog = GetComponent<LocalVolumetricFog>();
    }
    
    public void ChangeFog(float fogAmount)
    {
        localVolumetricFog.parameters.meanFreePath = fogAmount;
    }
}
