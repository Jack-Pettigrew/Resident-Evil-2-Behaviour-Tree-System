using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.Room;

namespace DD.AI.BehaviourTreeSystem
{
    public class BangDoor : LeafNode
    {
        public readonly string targetDoorBlackboardKey;
        
        public BangDoor(BehaviourTree behaviourTree, string targetDoorBlackboardKey) : base(behaviourTree)
        {
            this.targetDoorBlackboardKey = targetDoorBlackboardKey;
        }

        protected override NodeState Evaluate()
        {
            behaviourTree.Blackboard.GetFromBlackboard<Door>(targetDoorBlackboardKey).BangDoor();
            return NodeState.SUCCESSFUL;
        }
    }
}
