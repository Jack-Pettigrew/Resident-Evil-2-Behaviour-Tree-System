using UnityEngine;

namespace DD.AI.BehaviourTree
{
    public class DeleteSelfNode : Node
    {
        private Transform transform = null;

        public DeleteSelfNode(Transform transform)
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