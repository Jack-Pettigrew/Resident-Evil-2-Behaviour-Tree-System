using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DD.AI.Controllers;

namespace DD.AI.BehaviourTreeSystem
{
    public class MoveToNode : Node
    {
        private readonly IAIBehaviour ai;
        private readonly string targetBlackboardKey;

        // MOVEMENT
        private Transform moveTarget = null;

        // PATH CALCULATION
        private readonly float arrivedDistance;
        private NavMeshPath path;
        private int pathCornerIndex = 0;
        private const float NAV_WAYPOINT_THRESHOLD = 0.5f;

        private const float NAV_UPDATE_TIMER = 0.1f;
        private float timer = NAV_UPDATE_TIMER;

        public MoveToNode(IAIBehaviour ai, string targetBlackboardKey, float arrivedDistance)
        {
            this.ai = ai;
            this.targetBlackboardKey = targetBlackboardKey;
            this.arrivedDistance = arrivedDistance;
            path = new NavMeshPath();
            moveTarget = ai.GetAIBlackboard().GetFromBlackboard<Transform>(targetBlackboardKey);
        }

        public override NodeState Evaluate()
        {
            Debug.Log("MoveToNode");
            ai.MoveEvent(ai.GetAITransform().forward);
            return NodeState.SUCCESSFUL;

            if (!UpdatePath())
            {
                return NodeState.FAILED;
            }

            Vector3 navTargetDir = path.corners[pathCornerIndex] - ai.GetAITransform().position;
            float targetDist = (moveTarget.position - ai.GetAITransform().position).magnitude;

            //if(targetDist > arrivedDistance)
            //{
                if(navTargetDir.magnitude <= NAV_WAYPOINT_THRESHOLD)
                {
                    pathCornerIndex++;
                }

                ai.MoveEvent(navTargetDir);
                return NodeState.SUCCESSFUL;
                //return NodeState.RUNNING;
            //}
            //else
            //{
            //    return NodeState.SUCCESSFUL;
            //}
        }

        private bool UpdatePath()
        {
            timer -= Time.deltaTime;

            if(timer <= 0 || path.status == NavMeshPathStatus.PathInvalid)
            {
                // Check for current moveTarget
                if (!moveTarget)
                {
                    moveTarget = ai.GetAIBlackboard().GetFromBlackboard<Transform>(targetBlackboardKey);

                    // Fail if no move target in BlackBoard
                    if(!moveTarget)
                    {
                        return false;
                    }
                }

                NavMesh.CalculatePath(ai.GetAITransform().position, moveTarget.position, NavMesh.AllAreas, path);
                pathCornerIndex = 0;
                timer = NAV_UPDATE_TIMER;

                if(path.corners.Length == 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}