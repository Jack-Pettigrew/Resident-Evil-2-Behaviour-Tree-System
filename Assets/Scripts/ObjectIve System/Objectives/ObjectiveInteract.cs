using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems;

public class ObjectiveInteract : Objective
{
    public GameObject targetInteractable;
    
    public override void InitObjective()
    {
        GlobalEvents.OnInteract += EvaluateObjective;
    }

    public override void EvaluateObjective<GameObject>(GameObject interactedGameObject)
    {
        // Check it is the same
        if(Object.ReferenceEquals(targetInteractable, interactedGameObject))
        {
            CompleteObjective();
        }
    }

    public override void CleanUpObjective()
    {
        GlobalEvents.OnInteract -= EvaluateObjective;
    }
}
