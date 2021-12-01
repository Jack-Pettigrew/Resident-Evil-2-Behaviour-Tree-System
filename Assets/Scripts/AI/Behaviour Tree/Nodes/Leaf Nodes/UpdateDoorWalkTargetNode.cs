using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.Room;

namespace DD.AI.BehaviourTreeSystem
{
    public class UpdateDoorWalkTargetNode : UpdateBlackboardService
    {
        private readonly string targetDoorBlackboardKey;

        public UpdateDoorWalkTargetNode(BehaviourTree behaviourTree, string targetDoorBlackboardKey, string moveTargetrBlackboardKey) : base(behaviourTree, moveTargetrBlackboardKey)
        {
            this.targetDoorBlackboardKey = targetDoorBlackboardKey;
        }

        protected override NodeState Evaluate()
        {
            return UpdateBlackboard(behaviourTree.Blackboard.GetFromBlackboard<Door>(targetDoorBlackboardKey).GetExitPointRelativeToObject(behaviourTree.ai.GetAITransform().position)) 
                ? NodeState.SUCCESSFUL : NodeState.FAILED;
        }
    }
}