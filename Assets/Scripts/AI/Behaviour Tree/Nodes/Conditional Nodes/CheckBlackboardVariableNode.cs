using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.AI.Controllers;

namespace DD.AI.BehaviourTreeSystem
{
    public class CheckBlackboardVariableNode<T> : Conditional
    {
        private readonly string blackboardKey;
        private readonly T compareValue;
        private readonly ConditionType conditionType;

        public CheckBlackboardVariableNode(string blackboardKey, T compareValue, ConditionType conditionType, IAIBehaviour ai) : base(ai)
        {
            this.blackboardKey = blackboardKey;
            this.conditionType = conditionType;
            this.compareValue = compareValue;
        }

        public CheckBlackboardVariableNode(string blackboardKey, T compareValue, ConditionType conditionType, IAIBehaviour ai, Node trueNode, Node falseNode) : base(trueNode, falseNode)
        {
            this.blackboardKey = blackboardKey;
            this.conditionType = conditionType;
            this.ai = ai;
            this.compareValue = compareValue;
        }

        protected override NodeState EvaluateConditional()
        {
            T blackboardVariable = ai.GetAIBlackboard().GetFromBlackboard<T>(blackboardKey);

            switch (conditionType)
            {
                case ConditionType.Equals:
                    if(EqualityComparer<T>.Default.Equals(blackboardVariable, compareValue))
                    {
                        return NodeState.SUCCESSFUL;
                    }
                    break;
                default:
                    Debug.LogError($"{this}: other ConditionTypes have not yet been implmented in {typeof(CheckBlackboardVariableNode<T>).Name}.");
                    break;
            }
            return NodeState.FAILED;
        }
    }

    public enum ConditionType { Equals, GreaterThan, LessThan }
}