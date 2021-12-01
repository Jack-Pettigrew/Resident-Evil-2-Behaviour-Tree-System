using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DD.AI.BehaviourTreeSystem
{
    public class MoveToNode<T> : Node where T : Component
    {
        protected readonly string targetBlackboardKey;
        protected NavMeshPath path;

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

            behaviourTree.ai.MoveEvent(path.corners[1] - behaviourTree.ai.GetAITransform().position);
            return NodeState.SUCCESSFUL;
        }

        protected virtual Vector3 GetTargetPosition()
        {
            return behaviourTree.Blackboard.GetFromBlackboard<T>(targetBlackboardKey).transform.position;
        }

        protected bool UpdatePath()
        {
            // Fail if no target in BlackBoard
            if (behaviourTree.Blackboard.IsBlackboardVariableNull(targetBlackboardKey))
            {
                return false;
            }

            // Update Path
            NavMesh.CalculatePath(behaviourTree.ai.GetAITransform().position, GetTargetPosition(), NavMesh.AllAreas, path);

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