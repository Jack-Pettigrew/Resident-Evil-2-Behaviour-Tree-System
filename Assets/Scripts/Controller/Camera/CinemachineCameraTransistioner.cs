using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineBrain))]
public class CinemachineCameraTransistioner : MonoBehaviour
{
    [SerializeField, Min(0.1f)] private float transitionSpeed = 1.0f;
    private CinemachineBrain cinemachineBrain;
    [SerializeField] private CinemachineVirtualCamera cinemachinePlayVirtualCamera;

    private Coroutine transitionCoroutine = null;
    
    private void Awake() {
        cinemachineBrain = GetComponent<CinemachineBrain>();
    }
    
    public void TransitionCameraToSceneic(Transform sceneicTransform)
    {
        cinemachineBrain.enabled = false;

        if(transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }

        transitionCoroutine = StartCoroutine(TransitionTo(sceneicTransform));
    }

    [ContextMenu("Test Play View")]
    public void TransitionCameraToPlayView()
    {
        cinemachineBrain.enabled = false;

        if(transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }

        transitionCoroutine = StartCoroutine(TransitionTo(cinemachinePlayVirtualCamera.transform, () => {
            cinemachineBrain.enabled = true;
        }));
    }

    private IEnumerator TransitionTo(Transform targetTransform, Action callback = null)
    {
        float posDist = float.MaxValue, rotDiff = float.MaxValue;

        while(posDist > 0.02 || rotDiff > 0.02)
        {
            if(rotDiff > 0.02)
                transform.rotation = Quaternion.Slerp(transform.rotation, targetTransform.rotation, Time.deltaTime * transitionSpeed);

            if(posDist > 0.02)
                transform.position = Vector3.Lerp(transform.position, targetTransform.position, Time.deltaTime * transitionSpeed);

            posDist = Vector3.Distance(transform.position, targetTransform.position);
            rotDiff = Quaternion.Angle(transform.rotation, targetTransform.rotation);

            yield return null;
        }

        transitionCoroutine = null;
        
        if(callback != null)
        {
            callback();
        }
    }
}
