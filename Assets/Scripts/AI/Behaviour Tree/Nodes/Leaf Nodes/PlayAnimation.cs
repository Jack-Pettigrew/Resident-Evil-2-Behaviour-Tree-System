using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.AI.Controllers;

namespace DD.AI.BehaviourTreeSystem
{
    public class PlayAnimation : Node
    {
        private readonly string stateName;

        public PlayAnimation(BehaviourTree behaviourTree, string stateName) : base(behaviourTree)
        {
            this.stateName = stateName;
        }

        protected override NodeState Evaluate()
        {
            behaviourTree.ai.GetAnimator().Play(stateName);
            return NodeState.SUCCESSFUL;
        }
    }

}