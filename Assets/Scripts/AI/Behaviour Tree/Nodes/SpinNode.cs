using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    public class SpinNode : Node
    {
        private Transform ai = null;
        private float degrees = 0.0f;

        public SpinNode(BehaviourTree tree, Transform ai) : base(tree)
        {
            this.ai = ai;
        }

        public override NodeState Evaluate()
        {
            degrees = (degrees + 10.0f) % 360;
            ai.Rotate(Vector3.up, 10.0f);

            if (degrees != 0.0f)
            {
                return NodeState.RUNNING;
            }

            return NodeState.SUCCESSFUL;
        }
    }
}