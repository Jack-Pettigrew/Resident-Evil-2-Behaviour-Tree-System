using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.AI.BehaviourTreeSystem;

public class IdleNode : Node
{
    private bool isTemporary = false;
    private readonly float IDLE_LENGTH;
    private float timer = 0.0f;

    public IdleNode()
    {
    }

    public IdleNode(float time)
    {
        isTemporary = true;
        IDLE_LENGTH = time;
        timer = IDLE_LENGTH;
    }

    public override NodeState Evaluate()
    {
        if (isTemporary)
        {
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

        return NodeState.SUCCESSFUL;
    }
}
