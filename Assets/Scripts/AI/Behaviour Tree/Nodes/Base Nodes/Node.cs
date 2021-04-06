using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    [System.Serializable]
    public abstract class Node
    {
        public abstract NodeState Evaluate();
    }

    public enum NodeState
    {
        RUNNING,
        SUCCESSFUL,
        FAILED
    }
}