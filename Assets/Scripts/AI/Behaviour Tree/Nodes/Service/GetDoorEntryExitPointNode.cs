using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.Room;

namespace DD.AI.BehaviourTreeSystem
{
    public class GetDoorEntryExitPointNode : UpdateBlackboardService
    {
        private readonly string targetDoorBlackboardKey;
        private readonly bool entryPoint;

        /// <summary>
        /// Updates the Blackboad with the target Door's entry/exit point from the Blackboard.
        /// </summary>
        /// <param name="behaviourTree">The Behaviour Tree.</param>
        /// <param name="entryPoint">True: Get Door entry point. False: Get Door exit point.</param>
        /// <param name="targetDoorBlackboardKey">Key associated with the Target Door Blackboard variable.</param>
        /// <param name="moveTargetrBlackboardKey">Key associated with the Move Target Blackboard variable.</param>
        public GetDoorEntryExitPointNode(BehaviourTree behaviourTree, bool entryPoint, string targetDoorBlackboardKey, string moveTargetrBlackboardKey) : base(behaviourTree, moveTargetrBlackboardKey)
        {
            this.targetDoorBlackboardKey = targetDoorBlackboardKey;
            this.entryPoint = entryPoint;
        }

        protected override NodeState Evaluate()
        {
            if(entryPoint)
            {
                return UpdateBlackboard(behaviourTree.Blackboard.GetFromBlackboard<Door>(targetDoorBlackboardKey).GetEntryPointRelativeToObject(behaviourTree.ai.GetAITransform().position)) 
                    ? NodeState.SUCCESSFUL : NodeState.FAILED;
            }
            else
            {
                return UpdateBlackboard(behaviourTree.Blackboard.GetFromBlackboard<Door>(targetDoorBlackboardKey).GetExitPointRelativeToObject(behaviourTree.ai.GetAITransform().position))
                    ? NodeState.SUCCESSFUL : NodeState.FAILED;
            }
        }
    }
}