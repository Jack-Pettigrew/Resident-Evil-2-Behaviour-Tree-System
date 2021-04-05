using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    public class SetAIDestination : Node
    {
        string bbVariable = string.Empty;
        Action<Transform> aiSetDestination;

        public SetAIDestination(string bbVariable, Action<Transform> setDestinationCallback)
        {
            this.bbVariable = bbVariable;
            aiSetDestination = setDestinationCallback;
        }

        public override NodeState Evaluate()
        {
            object player;
            
            if(Blackboard.GetFromSharedBlackboardNonAlloc(bbVariable, out player))
            {
                aiSetDestination(((Transform)player));
                return NodeState.SUCCESSFUL;
            }
            else
            {
                return NodeState.FAILED;
            }

        }
    }
}