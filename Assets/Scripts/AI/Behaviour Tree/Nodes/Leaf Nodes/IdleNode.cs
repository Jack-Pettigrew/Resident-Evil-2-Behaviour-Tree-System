using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.AI.BehaviourTreeSystem;

public class IdleNode : Node
{
    private readonly float IDLE_LENGTH;
    private float timer = 0.0f;

    public IdleNode(BehaviourTree behaviourTree, float time) : base (behaviourTree)
    {
        IDLE_LENGTH = time;
        timer = IDLE_LENGTH;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("Idle");
        timer -= Time.deltaTime;

        if (timer <= 0.0f)
        {
            timer = IDLE_LENGTH;
            return NodeState.SUCCESSFUL;
        }
        else
        {
            return NodeState.RUNNING;
        }
    }
}
