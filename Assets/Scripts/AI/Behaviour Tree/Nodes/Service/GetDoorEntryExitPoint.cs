using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.Room;

namespace DD.AI.BehaviourTreeSystem
{
    public class GetDoorEntryExitPoint : UpdateBlackboardService
    {
        private readonly string moveTargetrBlackboardKey;
        private readonly string targetDoorBlackboardKey;
        private readonly bool entryPoint;

        /// <summary>
        /// Updates the Blackboad with the target Door's entry/exit point from the Blackboard.
        /// </summary>
        /// <param name="behaviourTree">The Behaviour Tree.</param>
        /// <param name="entryPoint">True: Get Door entry point. False: Get Door exit point.</param>
        /// <param name="targetDoorBlackboardKey">Key associated with the Target Door Blackboard variable.</param>
        /// <param name="moveTargetrBlackboardKey">Key associated with the Move Target Blackboard variable.</param>
        public GetDoorEntryExitPoint(BehaviourTree behaviourTree, bool entryPoint, string targetDoorBlackboardKey, string moveTargetrBlackboardKey) : base(behaviourTree)
        {
            this.moveTargetrBlackboardKey = moveTargetrBlackboardKey;
            this.targetDoorBlackboardKey = targetDoorBlackboardKey;
            this.entryPoint = entryPoint;
        }

        protected override bool UpdateBlackboard()
        {
            Door door = behaviourTree.Blackboard.GetFromBlackboard<Door>(targetDoorBlackboardKey);

            if(door)
            {
                if (entryPoint)
                {
                    return behaviourTree.Blackboard.UpdateBlackboardVariable(moveTargetrBlackboardKey, 
                        door.GetEntryPointRelativeToObject(behaviourTree.ai.GetAITransform().position));
                }
                else
                {
                    return behaviourTree.Blackboard.UpdateBlackboardVariable(moveTargetrBlackboardKey, 
                        door.GetExitPointRelativeToObject(behaviourTree.ai.GetAITransform().position));
                }
            }

            return false;
        }
    }
}