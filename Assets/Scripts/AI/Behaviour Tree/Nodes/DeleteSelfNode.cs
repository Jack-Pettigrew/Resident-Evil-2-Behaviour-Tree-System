using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    public class DeleteSelfNode : Node
    {
        private Transform transform = null;

        public DeleteSelfNode(BehaviourTree tree, Transform transform) : base(tree)
        {
            this.transform = transform;
        }

        public override NodeState Evaluate()
        {
            Object.Destroy(transform.gameObject);
            return NodeState.SUCCESSFUL;
        }
    }
}