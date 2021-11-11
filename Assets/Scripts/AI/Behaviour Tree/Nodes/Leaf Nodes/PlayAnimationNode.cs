using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.AI.Controllers;

namespace DD.AI.BehaviourTreeSystem
{
    public class PlayAnimationNode : Node
    {
        private readonly string stateName;

        public PlayAnimationNode(BehaviourTree behaviourTree, string stateName) : base(behaviourTree)
        {
            this.stateName = stateName;
        }

        public override NodeState Evaluate()
        {
            behaviourTree.ai.GetAnimator().Play(stateName);
            return NodeState.SUCCESSFUL;
        }
    }

}