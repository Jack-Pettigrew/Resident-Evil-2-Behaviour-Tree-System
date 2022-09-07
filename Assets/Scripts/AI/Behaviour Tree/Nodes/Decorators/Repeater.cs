using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    /// <summary>
    /// A Decorator that repeats it's children endlessly or until a condition is met. Behaviour Tree is still evaluated each tick.
    /// </summary>
    public class Repeater : Decorator
    {
        // Repeat Until Variables
        private bool repeatUntil = false;
        private Conditional untilNode = null;
        private NodeState breakRepeatState;

        /// <summary>
        /// Repeater endlessly repeating it's child node.
        /// </summary>
        /// <param name="behaviourTree">The Behaviour Tree</param>
        /// <param name="childNode">Child node to repeat.</param>
        public Repeater(BehaviourTree behaviourTree, Node childNode) : base(behaviourTree, childNode)
        { }

        /// <summary>
        /// Repeater repeating it's child node until the conditional node returns the given NodeState.
        /// </summary>
        /// <param name="behaviourTree">The Behaviour Tree</param>
        /// <param name="childNode">Child node to repeat.</param>
        /// <param name="untilNode">The conditional node to check whether the repeater should stop repeating.</param>
        /// <param name="breakRepeatState">The NodeState the Conditional node needs to return to end the repeater.</param>
        public Repeater(BehaviourTree behaviourTree, Node childNode, Conditional untilNode, NodeState breakRepeatState) : base(behaviourTree,childNode)
        {
            repeatUntil = true;
            this.untilNode = untilNode;
            this.breakRepeatState = breakRepeatState;
        }

        protected override NodeState Evaluate()
        {
            if (!repeatUntil)
            {
                childNode.UpdateNode();
                return NodeState.RUNNING;
            }
            else
            {
                childNode.UpdateNode();
                return untilNode.UpdateNode() == breakRepeatState ? NodeState.SUCCESSFUL : NodeState.RUNNING;
            }
        }
    }
}