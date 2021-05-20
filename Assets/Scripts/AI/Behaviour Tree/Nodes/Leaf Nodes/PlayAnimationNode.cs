using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.AI.Controllers;

namespace DD.AI.BehaviourTreeSystem
{
    public class PlayAnimationNode : Node
    {
        private readonly IAIBehaviour ai;
        private readonly string stateName;

        public PlayAnimationNode(string stateName, IAIBehaviour ai)
        {
            this.ai = ai;
            this.stateName = stateName;
        }

        public override NodeState Evaluate()
        {
            ai.GetAnimator().Play(stateName);
            return NodeState.SUCCESSFUL;
        }
    }

}