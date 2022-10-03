using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.AI.Controllers;
using DD.Animation.RigEvents;

namespace DD.AI.BehaviourTreeSystem
{
    public class SendAnimationRigSignal : LeafNode
    {
        private readonly int AnimationSignalReceiverLabelHash;
        private readonly AnimRigEventType eventType;
        
        public SendAnimationRigSignal(BehaviourTree behaviourTree, string animationSignalReceiverLabel, AnimRigEventType eventType, bool uninterruptable = false) : base(behaviourTree, uninterruptable)
        {
            AnimationSignalReceiverLabelHash = animationSignalReceiverLabel.GetHashCode();
            this.eventType = eventType;
        }

        protected override NodeState Evaluate()
        {
            behaviourTree.ai.GetAnimator().SendAnimRigEventSignal(AnimationSignalReceiverLabelHash, eventType);
            return NodeState.SUCCESSFUL;
        }
    }
}
