using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.AI.Controllers;

namespace DD.AI.BehaviourTreeSystem
{
    public class SetBlackboardVariableNode : Node
    {
        private readonly IAIBehaviour ai;
        private readonly string blackboardKey;
        private readonly object blackboardValue;

        public SetBlackboardVariableNode(IAIBehaviour aiInteractable, string blackboardKey, object blackboardValue)
        {
            this.blackboardKey = blackboardKey;
            this.blackboardValue = blackboardValue;
            ai = aiInteractable;
        }

        public override NodeState Evaluate()
        {
            return ai.GetAIBlackboard().UpdateBlackboardVariable(blackboardKey, blackboardValue) ? NodeState.SUCCESSFUL : NodeState.FAILED;
        }
    }

}