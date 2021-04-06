using DD.AI.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    public class MoveToPlayerNode : Node
    {
        private AIBeahviourTreeController ai = null;
        private Transform target = null;

        public MoveToPlayerNode(AIBeahviourTreeController ai)
        {
            this.ai = ai;

            object result;
            Blackboard.GetFromSharedBlackboardNonAlloc("Player", out result);
            target = (Transform)result;
        }

        public override NodeState Evaluate()
        {
            Vector3 targetDir = target.position - ai.transform.position;

            if(targetDir.sqrMagnitude >= 0.5f)
            {
                ai.MoveEvent(targetDir);
                return NodeState.RUNNING;
            }
            else
            {
                return NodeState.SUCCESSFUL;
            }

        }
    }
}