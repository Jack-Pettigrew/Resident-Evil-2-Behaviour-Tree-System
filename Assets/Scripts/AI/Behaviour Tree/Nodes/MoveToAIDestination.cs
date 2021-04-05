using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    public class MoveToAIDestination : Node
    {
        private Func<bool> aiMoveToDestination;

        public MoveToAIDestination(Func<bool> delegateCallback)
        {
            aiMoveToDestination = delegateCallback;
        }

        public override NodeState Evaluate()
        {
            return aiMoveToDestination() == false ? NodeState.RUNNING : NodeState.SUCCESSFUL;
        }
    }
}