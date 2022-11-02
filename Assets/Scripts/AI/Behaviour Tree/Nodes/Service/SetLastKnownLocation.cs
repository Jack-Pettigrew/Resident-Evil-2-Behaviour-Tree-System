using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    public class SetLastKnownLocation : UpdateBlackboardService
    {
        private readonly string lklBlackboardKey;
        private readonly string componentBlackboardKey;
        
        public SetLastKnownLocation(BehaviourTree behaviourTree, string lklBlackboardKey, string componentBlackboardKey) : base(behaviourTree)
        {
            this.lklBlackboardKey = lklBlackboardKey;
            this.componentBlackboardKey = componentBlackboardKey;
        }

        protected override bool UpdateBlackboard()
        {
            Component lklObject = behaviourTree.Blackboard.GetFromBlackboard<Component>(componentBlackboardKey);         
            
            return behaviourTree.Blackboard.UpdateBlackboardVariable(lklBlackboardKey, lklObject.transform.position, false);
        }
    }
}
