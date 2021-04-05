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
        private int pathCornerIndex = 0;
        private NavMeshPath path;
        private AIBeahviourTreeController ai;

        public MoveToAIDestination(AIBeahviourTreeController ai)
        {
            this.ai = ai;
            path = new NavMeshPath();
        }

        public override NodeState Evaluate()
        {
            RecalculatePath();
            if(path.corners.Length == 0)
            {
                return NodeState.RUNNING;
            }

            Vector3 targetDir = path.corners[pathCornerIndex] - ai.transform.position;
            float targetDist = targetDir.magnitude;

            if(targetDist > 1.0f)
            {
                ai.Move(targetDir);
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

        private const float RECALC_TIMER = .5f;
        private float timer = RECALC_TIMER;

        private void RecalculatePath()
        {
            timer -= Time.deltaTime;

            if(timer <= 0 || path.corners.Length == 0)
            {
                NavMesh.CalculatePath(ai.transform.position, ai.MoveTarget.position, NavMesh.AllAreas, path);
                pathCornerIndex = 0;

                timer = RECALC_TIMER;
            }
        }
    }
}