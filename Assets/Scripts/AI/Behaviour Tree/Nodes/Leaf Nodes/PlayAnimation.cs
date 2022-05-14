using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.AI.Controllers;

namespace DD.AI.BehaviourTreeSystem
{
    public class PlayAnimation : LeafNode
    {
        private readonly string stateName;
        private bool waitForAnimationToComplete;
        private bool waitingForAnimation = false;
        private bool waitComplete = true;

        public PlayAnimation(BehaviourTree behaviourTree, string stateName, bool waitForAnimationToComplete = false) : base(behaviourTree)
        {
            this.stateName = stateName;
            this.waitForAnimationToComplete = waitForAnimationToComplete;
        }

        protected override NodeState Evaluate()
        {
            if (waitForAnimationToComplete)
            {
                if (waitingForAnimation)
                {
                    if (waitComplete)
                    {
                        waitingForAnimation = false;
                        return NodeState.SUCCESSFUL;
                    }

                    return NodeState.RUNNING;
                }

                behaviourTree.ai.GetAnimator().PlayAndWait(stateName, StopWaiting);
                waitingForAnimation = true;
                waitComplete = false;
                return NodeState.RUNNING;
            }

            behaviourTree.ai.GetAnimator().Animator.SetTrigger(stateName);
            return NodeState.SUCCESSFUL;
        }

        private void StopWaiting()
        {
            waitComplete = true;
        }
    }
}