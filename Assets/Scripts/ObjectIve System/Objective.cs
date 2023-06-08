using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Objective
{
    [field: SerializeField] public string ObjectiveTitle { private set; get; }
    public bool IsComplete { private set; get; }
    public event Action OnObjectiveComplete;

    /// <summary>
    /// Initialises this Objective to register event based goals.
    /// </summary>
    public abstract void InitObjective();
    
    /// <summary>
    /// Evaluates the conditions which the objective is classed as completed.
    /// </summary>
    /// <param name="evaluationData">The data required to evaluate the objective.</param>
    public abstract void EvaluateObjective<T>(T evaluationData);

    /// <summary>
    /// Called by CompleteObjective - Cleans up any objective logic. Useful for when events need to be unsubscribed from.
    /// </summary>
    public virtual void CleanUpObjective()
    {
        // DEFAULT CLEANUP
    }

    /// <summary>
    /// Completes the Objective.
    /// </summary>
    public void CompleteObjective()
    {
        CleanUpObjective();
        IsComplete = true;
        OnObjectiveComplete?.Invoke();
    }
}
