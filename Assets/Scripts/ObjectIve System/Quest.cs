using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Core.Control;
using DD.Core.Items;

public class Quest : MonoBehaviour
{
    public string questName = "";
    [SerializeField] public bool IsComplete { private set; get; }

    private int currentObjectiveIndex = 0;
    [SerializeReference] private List<Objective> objectives;
    [SerializeReference] private List<ItemData> items;
    public Objective CurrentObjective { get { return objectives[currentObjectiveIndex]; } }

    // EVENTS
    public event Action<Quest> OnQuestProgressed;

    [ContextMenu("Add Interact Objective")]
    public void AddInteractObjective()
    {
        objectives.Add(new ObjectiveInteract());
    }

    [ContextMenu("Add Pickup Objective")]
    public void AddPickupObjective()
    {
        objectives.Add(new ObjectivePickup());
    }

    [ContextMenu("Add Arrive Objective")]
    public void AddArriveObjective()
    {
        objectives.Add(new ObjectiveArrive());
    }

    [ContextMenu("Start Test Quest")]
    public void StartQuest()
    {
        CurrentObjective.OnObjectiveComplete += ProgressToNextObjective;
        CurrentObjective.InitObjective();
    }

    public bool IsQuestComplete()
    {
        foreach (Objective objective in objectives)
        {
            if (!objective.IsComplete)
            {
                return false;
            }
        }

        return true;
    }

    public void ProgressToNextObjective()
    {        
        // Unsub from previous objective
        if (CurrentObjective.IsComplete)
        {
            CurrentObjective.OnObjectiveComplete -= ProgressToNextObjective;
        }

        if (IsQuestComplete())
        {
            EndQuest();
            return;
        }

        currentObjectiveIndex++;
        CurrentObjective.OnObjectiveComplete += ProgressToNextObjective;
        CurrentObjective.InitObjective();
        OnQuestProgressed?.Invoke(this);
    }

    public void EndQuest()
    {
        IsComplete = true;
    }
}
