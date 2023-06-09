using System;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public string questName = "";
    [field: SerializeField] public bool AutoProceedToNextObjective { private set; get; } = true;
    [SerializeField] public bool IsComplete { private set; get; }

    private int currentObjectiveIndex = 0;
    [SerializeReference] private List<Objective> objectives;
    public Objective CurrentObjective { get { return objectives[currentObjectiveIndex]; } }

    // EVENTS
    public static event Action<Objective> OnObjectiveComplete;
    public static event Action<Quest> OnQuestProgressed;

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
        if (AutoProceedToNextObjective)
        {
            CurrentObjective.OnObjectiveComplete.AddListener(AutoProgressToNextObjective);
        }

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

    public void ToggleAutoProceedObjectives(bool toggle)
    {
        if(AutoProceedToNextObjective == toggle) return;
        
        if(CurrentObjective.IsComplete)
        {
            AutoProgressToNextObjective();
        }
        else
        {
            CurrentObjective.OnObjectiveComplete.RemoveListener(AutoProgressToNextObjective);
        }
    }

    public void ProgressToNextObjective()
    {
        if (IsQuestComplete())
        {
            EndQuest();
            return;
        }

        currentObjectiveIndex++;
        CurrentObjective.InitObjective();
        OnQuestProgressed?.Invoke(this);
    }

    private void AutoProgressToNextObjective()
    {
        // Unsub from previous objective
        CurrentObjective.OnObjectiveComplete.RemoveListener(AutoProgressToNextObjective);

        if (IsQuestComplete())
        {
            EndQuest();
            return;
        }

        currentObjectiveIndex++;
        CurrentObjective.OnObjectiveComplete.AddListener(AutoProgressToNextObjective);
        CurrentObjective.InitObjective();
        OnQuestProgressed?.Invoke(this);
    }

    public void EndQuest()
    {
        IsComplete = true;
    }
}
