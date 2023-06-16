using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    public class CompareBlackboardVariable<T> : Conditional where T : IComparable
    {
        private readonly T comparerVariable;
        private readonly string blackboardVariableKey;
        private readonly CompareType compareType;

        public CompareBlackboardVariable(BehaviourTree behaviourTree, T comparerVariable, string blackboardVariableKey, CompareType compareType = CompareType.EQUALS) : base(behaviourTree)
        {
            this.comparerVariable = comparerVariable;
            this.blackboardVariableKey = blackboardVariableKey;
            this.compareType = compareType;
        }

        protected override NodeState EvaluateConditional()
        {
            switch (compareType)
            {
                case CompareType.EQUALS:
                    return behaviourTree.Blackboard.GetFromBlackboard<T>(blackboardVariableKey).Equals(comparerVariable) ? NodeState.SUCCESSFUL : NodeState.FAILED;

                case CompareType.COMPARE_EQUALS:
                    return behaviourTree.Blackboard.GetFromBlackboard<T>(blackboardVariableKey).CompareTo(comparerVariable) == 0 ? NodeState.SUCCESSFUL : NodeState.FAILED;

                case CompareType.LESS_THAN_EQUAL:
                    int compareValue = comparerVariable.CompareTo(behaviourTree.Blackboard.GetFromBlackboard<T>(blackboardVariableKey));
                    return (compareValue > 0 || compareValue == 0) ? NodeState.SUCCESSFUL : NodeState.FAILED;

                case CompareType.LESS_THAN:
                    return comparerVariable.CompareTo(behaviourTree.Blackboard.GetFromBlackboard<T>(blackboardVariableKey)) < 0 ? NodeState.SUCCESSFUL : NodeState.FAILED;

                default:
                    return NodeState.FAILED;
            }
        }
    }

    public enum CompareType
    {
        EQUALS,
        COMPARE_EQUALS,
        LESS_THAN_EQUAL,
        LESS_THAN
    }
}
