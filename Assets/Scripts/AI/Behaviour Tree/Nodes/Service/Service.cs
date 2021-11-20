using System.Collections;
using System.Collections.Generic;

namespace DD.AI.BehaviourTreeSystem
{
    /// <summary>
    /// Service nodes are nodes designed perform actions that affect the state of Behaviour Tree (e.g. updating variables in the Blackboard etc)
    /// rather than Leaf Nodes, which are nodes that affect the state of the AI.
    /// </summary>
    public abstract class Service : Node
    {
        public Service(BehaviourTree behaviourTree) : base(behaviourTree)
        { }
    }
}