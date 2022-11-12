using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.AI.Controllers;

namespace DD.AI.BehaviourTreeSystem
{
    public class MoveToVector3 : LeafNode
    {
        private readonly string targetBlackboardKey;

        private AILocomotion aiLocomotion;
        
        public MoveToVector3(BehaviourTree behaviourTree, string targetBlackboardKey) : base(behaviourTree)
        {
            this.targetBlackboardKey = targetBlackboardKey;
        }

        protected override bool OnStart()
        {
            if(aiLocomotion == null)
            {
                aiLocomotion = behaviourTree.ai.GetAIComponent<AILocomotion>();

                if(aiLocomotion == null)
                {
                    return false;
                }
            }

            return true;
        }

        protected override NodeState Evaluate()
        {            
            if (behaviourTree.Blackboard.IsBlackboardVariableNull(targetBlackboardKey))
            {
                return NodeState.FAILED;
            }
            
            if (!aiLocomotion.UpdatePath(behaviourTree.Blackboard.GetFromBlackboard<Vector3>(targetBlackboardKey)))
            {
                return NodeState.FAILED;
            }

            aiLocomotion.Move();

            return NodeState.SUCCESSFUL;
        }

    }
}
