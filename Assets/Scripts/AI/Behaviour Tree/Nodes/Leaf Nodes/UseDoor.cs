using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.Room;

namespace DD.AI.BehaviourTreeSystem
{
    public class UseDoor : Node
    {
        private readonly string targetDoorBlackboardKey;

        public UseDoor(BehaviourTree behaviourTree, string targetDoorBlackboardKey) : base(behaviourTree)
        {
        }

        protected override NodeState Evaluate()
        {
            behaviourTree.Blackboard.GetFromBlackboard<Door>(targetDoorBlackboardKey).OpenDoor();

            /*
             * A single Node for walking through a door is doing more than it needs to. Would benefit from being separated into individual components (e.g. 'WalkThroughDoor' > 'OpenDoor' + 'CloseDoor' + etc)
             * 
             * Separate into individual Nodes:
             * - OpenDoor
             * - CloseDoor
             * - Decide between:
             *      - GetOppositeDoorTarget
             *      - MoveTo
             *  - OR
             *      - Animation through Door
             *      - (may require thought into interuptable/uninteruptable Nodes
             *          - simple virtual bool that handles things and then returns true for success or false for it can't interupt?
             */

            // For now...
            return NodeState.SUCCESSFUL;
        }
    }
}