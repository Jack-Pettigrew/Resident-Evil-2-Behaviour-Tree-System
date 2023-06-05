using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.AI.Sensors;

namespace DD.AI.BehaviourTreeSystem
{
    public class HasHeardSound : Conditional
    {
        protected AIHear aiHear = null;
        protected bool heardSound = false;
        
        public HasHeardSound(BehaviourTree behaviourTree) : base(behaviourTree)
        {}

        protected override bool OnStart()
        {
            if(!base.OnStart()) return false;

            if(!aiHear)
            {
                aiHear = behaviourTree.ai.GetAIComponent<AIHear>();

                Debug.Log(aiHear, aiHear);
                
                if(!aiHear) return false;

                aiHear.OnSoundHeard += HeardSound;
            }

            return true;
        }

        protected void HeardSound()
        {
            heardSound = true;
        }

        protected override NodeState EvaluateConditional()
        {
            if(heardSound)
            {
                heardSound = false;
                return NodeState.SUCCESSFUL;
            }

            return NodeState.FAILED;
        }
    }
}
