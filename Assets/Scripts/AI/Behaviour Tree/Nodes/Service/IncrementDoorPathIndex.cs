using UnityEngine;
using DD.Systems.Room;

namespace DD.AI.BehaviourTreeSystem
{
    public class IncrementDoorPathIndex : UpdateBlackboardService
    {
        private readonly string doorIndexBlackboardKey;
        private readonly string doorPathArrayBlackboardKey;

        public IncrementDoorPathIndex(BehaviourTree behaviourTree, string doorIndexBlackboardKey, string doorPathArrayBlackboardKey) : base(behaviourTree)
        {
            this.doorIndexBlackboardKey = doorIndexBlackboardKey;
            this.doorPathArrayBlackboardKey = doorPathArrayBlackboardKey;
        }

        protected override bool UpdateBlackboard()
        {
            int index = behaviourTree.Blackboard.GetFromBlackboard<int>(doorIndexBlackboardKey) + 1;

            // If updating current door index by 1 excedes door array length, fail
            if (index % behaviourTree.Blackboard.GetFromBlackboard<Door[]>(doorPathArrayBlackboardKey).Length == 0)
            {
                return false;
            }
            else
            {
                return behaviourTree.Blackboard.UpdateBlackboardVariable(doorIndexBlackboardKey, index);
            }
        }
    } 
}