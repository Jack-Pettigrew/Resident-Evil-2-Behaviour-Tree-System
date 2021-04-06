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
        private const float NAV_UPDATE_TIMER = 0.25f;
        private float timer = NAV_UPDATE_TIMER;

        private float arrivedDistance = 1.0f;
        private int pathCornerIndex = 0;
        private NavMeshPath path;
        private AIBeahviourTreeController ai;

        public MoveToAIDestination(AIBeahviourTreeController ai, float arrivedDistance)
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

            Vector3 targetDir = path.corners[pathCornerIndex] - ai.transform.position;
            float targetDist = targetDir.magnitude;

            if(targetDist > arrivedDistance)
            {
                ai.MoveEvent(targetDir);
                return NodeState.RUNNING;
            }
            else
            {
                if (pathCornerIndex == path.corners.Length - 1)
                {
                    return NodeState.SUCCESSFUL;
                }
                
                pathCornerIndex++;
                return NodeState.RUNNING;
            }
        }

        private void RecalculatePath()
        {
            timer -= Time.deltaTime;

            if(timer <= 0 || path.status == NavMeshPathStatus.PathInvalid)
            {
                NavMesh.CalculatePath(ai.transform.position, ai.MoveTarget.position, NavMesh.AllAreas, path);
                pathCornerIndex = 0;

                Debug.Log(path.status);

                timer = NAV_UPDATE_TIMER;
            }
        }
    }
}