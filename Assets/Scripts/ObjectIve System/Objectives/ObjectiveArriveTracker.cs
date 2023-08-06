using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectiveArriveTracker : MonoBehaviour
{
    [SerializeField] private float arrivedRadius = 2.0f;
    private GameObject objectToTrack;
    private Coroutine trackingCoroutine;
    public UnityEvent<ObjectiveArriveTriggerStatus> OnTriggered;

    public void StartTracking(GameObject objectToTrack)
    {
        this.objectToTrack = objectToTrack;
        trackingCoroutine = StartCoroutine(UpdateTracking());
    }

    private IEnumerator UpdateTracking()
    {
        while (true)
        {
            if (Vector3.Distance(transform.position, objectToTrack.transform.position) <= arrivedRadius)
            {
                OnTriggered?.Invoke(ObjectiveArriveTriggerStatus.ARRIVED);
            }

            yield return null;
        }
    }

    public void StopTracking()
    {
        if (trackingCoroutine != null)
        {
            StopCoroutine(trackingCoroutine);
            objectToTrack = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, arrivedRadius);
    }
}

[System.Serializable]
public enum ObjectiveArriveTriggerStatus
{
    ARRIVED
}
