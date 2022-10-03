using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimConstraintTarget : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField, Tooltip("The layer the aim raycast should compare against.")] private LayerMask aimRigLayerMask;

    private void Update()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 1000.0f, aimRigLayerMask, QueryTriggerInteraction.Ignore);

        transform.position = hit.point == Vector3.zero ? playerCamera.gameObject.transform.position + playerCamera.gameObject.transform.forward * 10.0f : hit.point;
    }
}
