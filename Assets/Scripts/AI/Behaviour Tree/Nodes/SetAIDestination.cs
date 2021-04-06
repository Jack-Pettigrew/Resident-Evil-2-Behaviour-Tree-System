using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.AI.Controllers;

namespace DD.AI.BehaviourTreeSystem
{
    public class SetAIDestination : Node
    {
        private IAIBehaviour ai = null;
        private string targetBBName = string.Empty;

        public SetAIDestination(string bbVariable, IAIBehaviour ai)
        {
            this.targetBBName = bbVariable;
            this.ai = ai;
        }

        public override NodeState Evaluate()
        {
            object player;
            
            if(Blackboard.GetFromSharedBlackboardNonAlloc(targetBBName, out player))
            {
                ai.SetMoveTarget((Transform)player);
                return NodeState.SUCCESSFUL;
            }
            else
            {
                return NodeState.FAILED;
            }

        }
    }
}