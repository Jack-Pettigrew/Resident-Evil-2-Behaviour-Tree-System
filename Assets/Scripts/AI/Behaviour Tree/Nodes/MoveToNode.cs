using DD.AI.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTree
{
    public class MoveToNode : Node
    {
        private AIController ai = null;
        private Transform target = null;

        public MoveToNode(AIController ai)
        {
            this.ai = ai;

            object result;
            Blackboard.GetFromBlackboardNonAlloc("Player", out result);
            target = (Transform)result;
        }

        public override NodeState Evaluate()
        {
            Vector3 targetDir = target.position - ai.transform.position;

            if(targetDir.sqrMagnitude >= 0.5f)
            {
                ai.Move(targetDir);
                return NodeState.RUNNING;
            }
            else
            {
                return NodeState.SUCCESSFUL;
            }

        }
    }
}