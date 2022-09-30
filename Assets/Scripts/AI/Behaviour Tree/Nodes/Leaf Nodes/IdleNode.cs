using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.AI.BehaviourTreeSystem;

namespace DD.AI.BehaviourTreeSystem
{
    /// <summary>
    /// An Idle node - it literally does nothing for a specified amount of time
    /// </summary>
    public class IdleNode : LeafNode
    {
        private readonly float IDLE_LENGTH;
        private float timer = 0.0f;

        private bool infiniteIdle = false;

        public IdleNode(BehaviourTree behaviourTree) : base(behaviourTree)
        {
            infiniteIdle = true;
        }

        public IdleNode(BehaviourTree behaviourTree, string idleTimerBlackboardKey) : base(behaviourTree)
        {
            IDLE_LENGTH = behaviourTree.Blackboard.GetFromBlackboard<float>(idleTimerBlackboardKey);
            timer = IDLE_LENGTH;
        }

        protected override NodeState Evaluate()
        {
            if(infiniteIdle) {
                return NodeState.SUCCESSFUL;
            }
            
            timer -= Time.deltaTime;

            if (timer <= 0.0f)
            {
                timer = IDLE_LENGTH;
                return NodeState.SUCCESSFUL;
            }
            else
            {
                return NodeState.RUNNING;
            }
        }

    }
}
