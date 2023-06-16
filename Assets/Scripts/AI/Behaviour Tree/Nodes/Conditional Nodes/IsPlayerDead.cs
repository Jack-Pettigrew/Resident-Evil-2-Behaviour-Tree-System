using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Core.Control;
using DD.Core;

namespace DD.AI.BehaviourTreeSystem
{
    public class IsPlayerDead : Conditional
    {
        private Health health;
        
        public IsPlayerDead(BehaviourTree behaviourTree) : base(behaviourTree)
        {
            if(Object.FindObjectOfType<PlayerController>().TryGetComponent<Health>(out health))
            {
                Debug.LogWarning("Unable to find Player Health component. Make sure the GameObject with PlayerController component has a Health component.");
            }
        }

        protected override NodeState EvaluateConditional()
        {
            if(!health) return NodeState.FAILED;

            return health.IsDead ? NodeState.SUCCESSFUL : NodeState.FAILED;
        }
    }
}
