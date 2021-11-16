using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.AI.Controllers;

namespace DD.AI.BehaviourTreeSystem
{
    public class CanSeePlayerNode : Conditional
    {
        private readonly string fovAngleBlackboardKey;
        private readonly string fovRangeBlackboardKey;
        private readonly string playerLayerMaskBlackboardKey;
        private readonly string environmentLayerMaskBlackboardKey;

        Collider[] playerColliders;

        public CanSeePlayerNode(BehaviourTree behaviourTree, string fovAngleBlackboardKey, string fovRangeBlackboardKey, string playerLayerMaskBlackboardKey, string environmentLayerMaskBlackboardKey) : base(behaviourTree)
        {
            this.fovAngleBlackboardKey = fovAngleBlackboardKey;
            this.fovRangeBlackboardKey = fovRangeBlackboardKey;
            this.playerLayerMaskBlackboardKey = playerLayerMaskBlackboardKey;
            this.environmentLayerMaskBlackboardKey = environmentLayerMaskBlackboardKey;

            // Preallocated array for OverlapSphereNonAlloc
            // - Set to 4 in the event their are multiple players
            playerColliders = new Collider[4];
        }

        protected override NodeState EvaluateConditional()
        {
            // Vicinity
            Physics.OverlapSphereNonAlloc(behaviourTree.ai.GetAITransform().position, behaviourTree.Blackboard.GetFromBlackboard<float>(fovRangeBlackboardKey), playerColliders, behaviourTree.Blackboard.GetFromBlackboard<LayerMask>(playerLayerMaskBlackboardKey), QueryTriggerInteraction.Ignore);
            
            foreach (var collider in playerColliders)
            {
                if (!collider) continue;

                Vector3 targetDir = collider.transform.position - behaviourTree.ai.GetAITransform().position;

                // Is player within view cone?
                if (Mathf.Abs(Vector3.Angle(behaviourTree.ai.GetAITransform().forward, targetDir)) <= behaviourTree.Blackboard.GetFromBlackboard<float>(fovAngleBlackboardKey))
                {
                    // Is view to player unobsctructed?
                    if (!Physics.Raycast(behaviourTree.ai.GetAITransform().position, targetDir, behaviourTree.Blackboard.GetFromBlackboard<float>(fovRangeBlackboardKey), behaviourTree.Blackboard.GetFromBlackboard<LayerMask>(environmentLayerMaskBlackboardKey), QueryTriggerInteraction.Ignore))
                    {
                        return NodeState.SUCCESSFUL;
                    }
                }
            }

            return NodeState.FAILED;
        }
    }
}