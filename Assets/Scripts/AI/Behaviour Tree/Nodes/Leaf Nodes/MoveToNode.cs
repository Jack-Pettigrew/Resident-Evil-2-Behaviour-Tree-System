using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DD.AI.BehaviourTreeSystem
{
    public class MoveToNode : Node
    {
        private readonly string targetBlackboardKey;
        private NavMeshPath path;

        public MoveToNode(BehaviourTree behaviourTree, string targetBlackboardKey) : base(behaviourTree)
        {
            this.targetBlackboardKey = targetBlackboardKey;
            path = new NavMeshPath();
        }

        protected override NodeState Evaluate()
        {
            if (!UpdatePath())
            {
                return NodeState.FAILED;
            }

            Vector3 navTargetDir = path.corners[1] - behaviourTree.ai.GetAITransform().position;
            float targetDist = (behaviourTree.Blackboard.GetFromBlackboard<GameObject>(targetBlackboardKey).transform.position - behaviourTree.ai.GetAITransform().position).magnitude;

            behaviourTree.ai.MoveEvent(navTargetDir);
            return NodeState.SUCCESSFUL;
        }

        private bool UpdatePath()
        {
            // Fail if no target in BlackBoard
            if (!behaviourTree.Blackboard.GetFromBlackboard<GameObject>(targetBlackboardKey))
            {
                return false;
            }

            // Update Path
            NavMesh.CalculatePath(behaviourTree.ai.GetAITransform().position, behaviourTree.Blackboard.GetFromBlackboard<GameObject>(targetBlackboardKey).transform.position, NavMesh.AllAreas, path);

            // Fail if path is invalid
            if(path.corners.Length <= 0)
            {
                return false;
            }

            #if UNITY_EDITOR
            for (int i = 0; i < path.corners.Length; i++)
            {
                if (i + 1 == path.corners.Length) break;

                Debug.DrawLine(path.corners[i], path.corners[i + 1]);
            }
            #endif

            return true;
        }
    }
}