using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems;
using DD.Core.Items;

public class ObjectiveArrive : Objective
{
    [SerializeField] private GameObject trackingGameObject;
    [SerializeField] private ObjectiveArriveTriggerStatus targetStatus;
    public ObjectiveArriveTracker arriveTrigger;
    
    public override void InitObjective()
    {
        arriveTrigger.OnTriggered += EvaluateObjective;
        arriveTrigger.StartTracking(trackingGameObject);
    }

    public override void EvaluateObjective<ObjectiveArriveTriggerStatus>(ObjectiveArriveTriggerStatus status)
    {       
        if(targetStatus.CompareTo(status) == 0)
        {
            CompleteObjective();
        }
    }

    public override void CleanUpObjective()
    {
        arriveTrigger.OnTriggered -= EvaluateObjective;
        arriveTrigger.StopTracking();
    }
}

