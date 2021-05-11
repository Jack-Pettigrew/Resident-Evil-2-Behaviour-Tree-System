using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    public class WaitNode : Node
    {
        private float timer = 0.0f;
        private readonly float waitDuration;

        /// <summary>
        /// Wait Node: more or less a Behaviour Tree idle node.
        /// </summary>
        /// <param name="duration">Duration to wait.</param>
        WaitNode(float duration)
        {
            waitDuration = duration;
        }

        public override NodeState Evaluate()
        {
            timer += Time.deltaTime;

            if(timer < waitDuration)
            {
                return NodeState.RUNNING;
            }
            else
            {
                Reset();
                return NodeState.SUCCESSFUL;
            }
        }

        private void Reset()
        {
            timer = 0.0f;
        }
    }
}
