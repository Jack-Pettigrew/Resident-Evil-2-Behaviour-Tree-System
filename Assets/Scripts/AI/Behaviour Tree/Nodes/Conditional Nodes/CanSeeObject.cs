using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.AI.Controllers;
using DD.AI.Sensors;

namespace DD.AI.BehaviourTreeSystem
{    
    public class CanSeeObject : Conditional
    {
        private AIVision aiVision;
        
        public CanSeeObject(BehaviourTree behaviourTree) : base(behaviourTree)
        { }

        protected override bool OnStart()
        {
            if(aiVision == null)
            {
                aiVision = behaviourTree.ai.GetAIComponent<AIVision>();

                if(aiVision == null)
                {
                    Debug.LogError("AI does not have an AIVision component!");
                    return false;
                }
            }

            return true;
        }

        protected override NodeState EvaluateConditional()
        {
            return aiVision.Sense() ? NodeState.SUCCESSFUL : NodeState.FAILED;
        }
    }
}