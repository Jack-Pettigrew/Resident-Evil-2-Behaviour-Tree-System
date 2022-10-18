using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimConstraintTarget : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField, Tooltip("The layer the aim raycast should compare against.")] private LayerMask aimRigLayerMask;

    private float defaultTargetDistance = 10.0f;
    private Vector3 aimTarget = Vector3.zero;
    [SerializeField] private float targetChangeSpeed = 2.0f;

    private void Update()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 1000.0f, aimRigLayerMask, QueryTriggerInteraction.Ignore))
        {
            aimTarget = hit.point;
        }
        else
        {
            aimTarget = playerCamera.gameObject.transform.position + playerCamera.gameObject.transform.forward * defaultTargetDistance;
        }

        transform.position = Vector3.Lerp(transform.position, aimTarget, Time.deltaTime * targetChangeSpeed);
        // hit.point == Vector3.zero ? playerCamera.gameObject.transform.position + playerCamera.gameObject.transform.forward * 10.0f : hit.point;
    }
}
