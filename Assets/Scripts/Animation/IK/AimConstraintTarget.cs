using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimConstraintTarget : MonoBehaviour
{
    [SerializeField] private float targetDistance = 5.0f;
    [SerializeField] private Transform cameraTransform;

    private void Update()
    {
        if(cameraTransform)
        {
            transform.position = cameraTransform.position + cameraTransform.transform.forward * targetDistance;
        }
    }
}
