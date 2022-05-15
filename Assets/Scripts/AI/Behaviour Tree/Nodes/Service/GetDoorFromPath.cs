using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DD.Systems.Room;

namespace DD.AI.BehaviourTreeSystem
{
    public class GetDoorFromDoorPath : UpdateBlackboardService
    {
        private readonly string doorPathBlackboardKey;
        private readonly string doorPathIndexBlackboardKey;
        private readonly string targetBlackboardKey;

        public GetDoorFromDoorPath(BehaviourTree behaviourTree, string doorPathBlackboardKey, string doorPathIndexBlackboardKey, string targetBlackboardKey) : base(behaviourTree)
        {
            this.doorPathBlackboardKey = doorPathBlackboardKey;
            this.doorPathIndexBlackboardKey = doorPathIndexBlackboardKey;
            this.targetBlackboardKey = targetBlackboardKey;
        }

        protected override bool UpdateBlackboard()
        {
            if (!behaviourTree.Blackboard.IsBlackboardVariableNull(doorPathBlackboardKey))
            {
                return behaviourTree.Blackboard.UpdateBlackboardVariable(targetBlackboardKey,
                    behaviourTree.Blackboard.GetFromBlackboard<Door[]>(doorPathBlackboardKey)[
                        behaviourTree.Blackboard.GetFromBlackboard<int>(doorPathIndexBlackboardKey)
                    ]
                );
            }

            return false;
        }
    }
}
