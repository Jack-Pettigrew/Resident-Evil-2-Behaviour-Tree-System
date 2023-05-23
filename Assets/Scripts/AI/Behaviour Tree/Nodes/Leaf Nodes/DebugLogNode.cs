using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.AI.BehaviourTreeSystem;

public class DebugLogNode : LeafNode
{
    private readonly string log;
    
    public DebugLogNode(BehaviourTree behaviourTree, string log) : base(behaviourTree)
    {
        this.log = log;
    }
    
    protected override NodeState Evaluate()
    {
        Debug.Log(log);

        return NodeState.SUCCESSFUL;
    }
}
