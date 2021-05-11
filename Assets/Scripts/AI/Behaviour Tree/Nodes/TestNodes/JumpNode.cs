using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.AI.Controllers;

namespace DD.AI.BehaviourTreeSystem
{
    public class JumpNode : Node
    {
        private AIBeahviourTreeController ai = null;

        private const float TIMERLENGTH = 3.0f;
        private float timer = TIMERLENGTH;

        public JumpNode(AIBeahviourTreeController ai)
        {
            this.ai = ai;
        }

        public override NodeState Evaluate()
        {
            timer -= Time.deltaTime;

            if(timer <= 0.0f)
            {
                timer = TIMERLENGTH;
                ai.transform.position += Vector3.up * 2.0f;

                return NodeState.SUCCESSFUL;
            }

            return NodeState.RUNNING;
        }
    }
}