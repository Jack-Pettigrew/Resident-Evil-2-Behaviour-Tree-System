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
        private readonly string targetBlackboardKey;

        // MOVEMENT
        private Transform moveTarget = null;

        // PATH CALCULATION
        private readonly float arrivedDistance;
        private NavMeshPath path;
        private int pathCornerIndex = 0;
        private const float NAV_WAYPOINT_THRESHOLD = 0.5f;

        private const float NAV_UPDATE_TIMER = 0.02f;
        private float timer = NAV_UPDATE_TIMER;

        public MoveToNode(BehaviourTree behaviourTree, string targetBlackboardKey, float arrivedDistance) : base(behaviourTree)
        {
            this.targetBlackboardKey = targetBlackboardKey;
            this.arrivedDistance = arrivedDistance;
            path = new NavMeshPath();
            moveTarget = behaviourTree.Blackboard.GetFromBlackboard<Transform>(targetBlackboardKey);
        }

        public override NodeState Evaluate()
        {
            if (!UpdatePath())
            {
                return NodeState.FAILED;
            }

            Vector3 navTargetDir = path.corners[pathCornerIndex] - behaviourTree.ai.GetAITransform().position;
            float targetDist = (moveTarget.position - behaviourTree.ai.GetAITransform().position).magnitude;

            //if(targetDist > arrivedDistance)
            //{
                if(navTargetDir.magnitude <= NAV_WAYPOINT_THRESHOLD)
                {
                    pathCornerIndex++;
                }

                behaviourTree.ai.MoveEvent(navTargetDir);
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
                    moveTarget = behaviourTree.Blackboard.GetFromBlackboard<Transform>(targetBlackboardKey);

                    // Fail if no move target in BlackBoard
                    if(!moveTarget)
                    {
                        return false;
                    }
                }

                NavMesh.CalculatePath(behaviourTree.ai.GetAITransform().position, moveTarget.position, NavMesh.AllAreas, path);
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