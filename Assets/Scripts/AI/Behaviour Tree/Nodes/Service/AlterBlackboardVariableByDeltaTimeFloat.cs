using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    public class AlterBlackboardVariableByDeltaTimeFloat : Service
    {
        private readonly string blackboardVariableKey;
        private readonly AffectValueType affectValueType;
        
        public AlterBlackboardVariableByDeltaTimeFloat(BehaviourTree behaviourTree, string blackboardVariableKey, AffectValueType affectValueType, bool uninterruptable = false) : base(behaviourTree, uninterruptable)
        {
            this.blackboardVariableKey = blackboardVariableKey;
            this.affectValueType = affectValueType;
        }

        protected override NodeState Evaluate()
        {
            float variable = behaviourTree.Blackboard.GetFromBlackboard<float>(blackboardVariableKey);
            variable += (affectValueType == AffectValueType.INCREMENT ? Time.deltaTime : -Time.deltaTime);
            return behaviourTree.Blackboard.UpdateBlackboardVariable(blackboardVariableKey, variable) ? NodeState.SUCCESSFUL : NodeState.FAILED;
        }
    }

    public enum AffectValueType
    {
        INCREMENT,
        DECREMENT
    }
}
