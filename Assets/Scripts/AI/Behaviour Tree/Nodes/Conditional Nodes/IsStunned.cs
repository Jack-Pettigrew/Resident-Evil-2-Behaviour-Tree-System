using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Core;

namespace DD.AI.BehaviourTreeSystem
{
    public class IsStunned : LeafNode
    {
        private StunManager stunManager;
        
        public IsStunned(BehaviourTree behaviourTree) : base(behaviourTree)
        {
            stunManager = behaviourTree.ai.GetAIComponent<StunManager>();
        }
        
        protected override NodeState Evaluate()
        {
            return stunManager.IsStunned ? NodeState.SUCCESSFUL : NodeState.FAILED;
        }
    }
}
