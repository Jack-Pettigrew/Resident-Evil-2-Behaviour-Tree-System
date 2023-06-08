using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems;
using DD.Core.Items;

public class ObjectivePickup : Objective
{
    public ItemData targetItemData;
    
    public override void InitObjective()
    {
        GlobalEvents.OnPickupItem += EvaluateObjective;
    }

    public override void EvaluateObjective<ItemData>(ItemData pickedupItem)
    {       
        // Check it is the same
        if(Object.ReferenceEquals(pickedupItem, targetItemData))
        {
            CompleteObjective();
        }
    }

    public override void CleanUpObjective()
    {
        GlobalEvents.OnPickupItem -= EvaluateObjective;
    }
}