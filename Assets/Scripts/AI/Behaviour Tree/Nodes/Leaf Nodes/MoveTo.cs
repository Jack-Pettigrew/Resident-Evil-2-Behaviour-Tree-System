using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.AI.Controllers;

namespace DD.AI.BehaviourTreeSystem
{    
    public class MoveTo<T> : LeafNode where T : Component
    {
        private readonly string targetBlackboardKey;

        private AILocomotion aiLocomotion;
        
        public MoveTo(BehaviourTree behaviourTree, string targetBlackboardKey) : base(behaviourTree)
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
            
            if (!aiLocomotion.UpdatePath(behaviourTree.Blackboard.GetFromBlackboard<T>(targetBlackboardKey).transform.position))
            {
                return NodeState.FAILED;
            }

            aiLocomotion.Move();

            return NodeState.SUCCESSFUL;
        }
    }
}