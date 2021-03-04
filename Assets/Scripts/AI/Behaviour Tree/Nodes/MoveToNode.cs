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
            this.target = (Transform)Blackboard.GetFromBlackboard("Player");
        }

        public override NodeState Evaluate()
        {
            Vector3 targetDir = ai.transform.position - target.position;

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