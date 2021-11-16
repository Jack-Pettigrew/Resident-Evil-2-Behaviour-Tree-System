using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.AI.BehaviourTreeSystem;

public class IdleNode : Node
{
    private readonly float IDLE_LENGTH;
    private float timer = 0.0f;

    public IdleNode(BehaviourTree behaviourTree, string idleTimerBlackboardKey) : base (behaviourTree)
    {
        IDLE_LENGTH = behaviourTree.Blackboard.GetFromBlackboard<float>(idleTimerBlackboardKey);
        timer = IDLE_LENGTH;
    }

    public override NodeState Evaluate()
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
}
