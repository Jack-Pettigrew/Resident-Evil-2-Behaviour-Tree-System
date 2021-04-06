using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DD.AI.Controllers;

namespace DD.AI.BehaviourTreeSystem
{
    public class MoveToAIDestination : Node
    {
        private const float NAV_UPDATE_TIMER = 0.1f;
        private float timer = NAV_UPDATE_TIMER;

        private float navTargetDistThreshold = 0.5f;
        private Vector3 moveTarget = Vector3.zero;
        private float arrivedDistance = 1.0f;
        private int pathCornerIndex = 0;
        private NavMeshPath path;

        private IAIBehaviour ai;

        public MoveToAIDestination(IAIBehaviour ai, float arrivedDistance)
        {
            this.ai = ai;
            this.arrivedDistance = arrivedDistance;
            path = new NavMeshPath();
        }

        public override NodeState Evaluate()
        {
            // Pathing
            RecalculatePath();
            if(path.status == NavMeshPathStatus.PathInvalid)
            {
                return NodeState.RUNNING;
            }

            Vector3 navTargetDir = path.corners[pathCornerIndex] - ai.GetAITransform().position;
            float navTargetDist = navTargetDir.magnitude;
            float targetDist = (moveTarget - ai.GetAITransform().position).magnitude;

            if(targetDist > arrivedDistance)
            {
                if(navTargetDist <= navTargetDistThreshold)
                {
                    pathCornerIndex++;
                }

                ai.MoveEvent(navTargetDir);
                return NodeState.RUNNING;
            }
            else
            {
                return NodeState.SUCCESSFUL;
            }
        }

        private void RecalculatePath()
        {
            timer -= Time.deltaTime;

            if(timer <= 0 || path.status == NavMeshPathStatus.PathInvalid)
            {
                moveTarget = ai.GetAIMoveTarget().position;

                NavMesh.CalculatePath(ai.GetAITransform().position, moveTarget, NavMesh.AllAreas, path);
                pathCornerIndex = 0;

                Debug.Log(path.status);

                timer = NAV_UPDATE_TIMER;
            }
        }
    }
}