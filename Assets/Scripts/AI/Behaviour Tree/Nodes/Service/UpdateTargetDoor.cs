using DD.Systems.Room;

namespace DD.AI.BehaviourTreeSystem
{
    public class UpdateTargetDoor : UpdateBlackboardService
    {
        private readonly string doorPathArrayBlackboardKey;

        public UpdateTargetDoor(BehaviourTree behaviourTree, string doorIndexBlackboardKey, string doorPathArrayBlackboardKey) : base(behaviourTree, doorIndexBlackboardKey)
        {
            this.doorPathArrayBlackboardKey = doorPathArrayBlackboardKey;
        }

        protected override NodeState Evaluate()
        {
            // If updating current door index by 1 excedes door array length, fail
            if ((behaviourTree.Blackboard.GetFromBlackboard<int>(variableBlackboardKey) + 1) % behaviourTree.Blackboard.GetFromBlackboard<Door[]>(doorPathArrayBlackboardKey).Length == 0)
            {
                return NodeState.FAILED;
            }
            else
            {
                UpdateBlackboard(behaviourTree.Blackboard.GetFromBlackboard<int>(variableBlackboardKey) + 1);
                return NodeState.SUCCESSFUL;
            }
        }
    } 
}