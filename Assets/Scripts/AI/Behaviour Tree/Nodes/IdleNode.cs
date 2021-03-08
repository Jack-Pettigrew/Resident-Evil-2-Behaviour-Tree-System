using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.AI.BehaviourTreeSystem;

public class IdleNode : Node
{
    private float idleLength = 5.0f;
    private float timer = 0.0f;

    public IdleNode(BehaviourTree tree) : base(tree)
    {
        timer = idleLength;
    }

    public override NodeState Evaluate()
    {
        timer -= Time.deltaTime;

        if(timer <= 0.0f)
        {
            timer = idleLength;
            return NodeState.SUCCESSFUL;
        }
        else
            return NodeState.RUNNING;
    }
}
